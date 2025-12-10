using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(AudioSource), typeof(CanvasGroup))]
[DisallowMultipleComponent]
public class Page : MonoBehaviour
{
    public bool exitOnNewPagePush = false;
    [Tooltip("If enabled, this page will automatically load in at the center of the scene when the scene loads. ")]
    [SerializeField] private bool posLoadInAtCenter;
    private bool alreadyLoaded;

    [Header("References")]
    [SerializeField] private AudioClip entryClip;
    [SerializeField] private AudioClip exitClip;
    private AudioSource audioSource;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    [Header("Animation")]
    [SerializeField] private float animationSpeed = 4f;
    [SerializeField] private EntryMode entryMode = EntryMode.SLIDE;
    [Tooltip("entryStartPos is the position where this UI element will start OUTSIDE of the sreen view")]
    [SerializeField] private Vector2 entryStartPos = Vector2.zero;
    [Tooltip("If enabled, the entryStartPos will use this UI element's current position")]
    [SerializeField] private bool startingPositionEqualsCurrentPosition = true;
    [Tooltip("entryEndPos is the position where this UI element will end up INSIDE the screen view")]
    [SerializeField] private Vector2 entryEndPos = Vector2.zero;
    [SerializeField] private EntryMode exitMode = EntryMode.SLIDE;
    [Tooltip("exitEndPos is the position where this UI element will transition to OUTSIDE the screen view")]
    [SerializeField] private Vector2 exitEndPos = Vector2.zero;
    [Tooltip("If enabled, the exitEndPos will use this UI element's current position")]
    [SerializeField] private bool endingPositionEqualsCurrentPosition;

    private Coroutine animationCoroutine;
    private Coroutine audioCoroutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0;
        audioSource.enabled = false;

        if (startingPositionEqualsCurrentPosition)
            entryStartPos = rectTransform.anchoredPosition;

        if (endingPositionEqualsCurrentPosition)
            exitEndPos = rectTransform.anchoredPosition;

        alreadyLoaded = false;
    }

    public void Enter(bool playAudio)
    {
        if (!alreadyLoaded && posLoadInAtCenter)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            alreadyLoaded = true;
            return;
        }

        switch (entryMode)
        {
            case EntryMode.SLIDE:
                SlideIn(playAudio);
                break;
            case EntryMode.ZOOM:
                ZoomIn(playAudio);
                break;
            case EntryMode.FADE:
                FadeIn(playAudio);
                break;
        }
    }

    public void Exit(bool playAudio)
    {
        switch (exitMode)
        {
            case EntryMode.SLIDE:
                SlideOut(playAudio);
                break;
            case EntryMode.ZOOM:
                ZoomOut(playAudio);
                break;
            case EntryMode.FADE:
                FadeOut(playAudio);
                break;
        }
    }

    private void SlideIn(bool playAudio)
    {
        rectTransform.anchoredPosition = entryStartPos;
        AnimationHelper.SlideIn(rectTransform, entryEndPos, animationSpeed, null);

        PlayEntryClip(playAudio);
    }

    private void SlideOut(bool playAudio)
    {
        AnimationHelper.SlideOut(rectTransform, exitEndPos, animationSpeed, null);

        PlayExitClip(playAudio);
    }

    private void ZoomIn(bool playAudio)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(AnimationHelper.ZoomIn(rectTransform, animationSpeed, null));

        PlayEntryClip(playAudio);
    }

    private void ZoomOut(bool playAudio)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(AnimationHelper.ZoomOut(rectTransform, animationSpeed, null));

        PlayExitClip(playAudio);
    }

    private void FadeIn(bool playAudio)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(AnimationHelper.FadeIn(canvasGroup, animationSpeed, null));

        PlayEntryClip(playAudio);
    }

    private void FadeOut(bool playAudio)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(AnimationHelper.FadeOut(canvasGroup, animationSpeed, null));

        PlayExitClip(playAudio);
    }

    private void PlayEntryClip(bool playAudio)
    {
        if (playAudio && entryClip != null && audioSource != null)
        {
            if (audioCoroutine != null)
                StopCoroutine(audioCoroutine);

            audioCoroutine = StartCoroutine(PlayClip(entryClip));
        }
    }

    private void PlayExitClip(bool playAudio)
    {
        if (playAudio && exitClip != null && audioSource != null)
        {
            if (audioCoroutine != null)
                StopCoroutine(audioCoroutine);

            audioCoroutine = StartCoroutine(PlayClip(exitClip));
        }
    }

    private IEnumerator PlayClip(AudioClip clip)
    {
        audioSource.enabled = true;

        WaitForSeconds wait = new WaitForSeconds(clip.length);
        audioSource.PlayOneShot(clip);
        yield return wait;

        audioSource.enabled = false; // Disabling audio source when not playing eliminates some CPU overhead
    }
}
