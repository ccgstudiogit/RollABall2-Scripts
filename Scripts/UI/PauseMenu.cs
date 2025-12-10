using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEditor;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private RectTransform rectTransform;

    [Header("Animation Settings")]
    [Tooltip("The position where the pause menu will start and then transition to entryEndPos")]
    [SerializeField] private Vector2 entryStartPos = Vector2.zero;
    [Tooltip("The position where the pause menu will go when disabled")]
    [SerializeField] private Vector2 entryEndPos = Vector2.zero;
    [Tooltip("The position the pause menu will go when enabled")]
    [SerializeField] private Vector2 exitEndPos = Vector2.zero;
    [SerializeField] private float animationTime = 0.25f;

    [Header("Sound Effect")]
    [SerializeField] private SoundEffectSO soundEffect;
    [SerializeField] private AudioSource audioSource;
    private bool ableToPlaySFX;

    private void Awake()
    {
        if (pauseMenu == null || rectTransform == null)
        {
            Debug.LogError("PauseMenu's pauseMenu or rectTransform null. Please assign a reference. Disabling this component.");
            enabled = false;
        }
        
        if (soundEffect != null && audioSource != null)
            ableToPlaySFX = true;
        else if (soundEffect != null && audioSource == null)
            Debug.LogWarning("PauseMenu has a sound effect but no audioSource reference. Unable to play sound effect.");
    }

    private void OnEnable()
    {
        GameController.OnPause += EnablePauseMenu;
        GameController.OnResume += DisablePauseMenu;
    }

    private void OnDisable()
    {
        GameController.OnPause -= EnablePauseMenu;
        GameController.OnResume -= DisablePauseMenu;
    }

    private void EnablePauseMenu()
    {
        if (ableToPlaySFX)
            soundEffect.Play(audioSource);

        rectTransform.anchoredPosition = entryStartPos;

        pauseMenu.SetActive(true);

        rectTransform.DOAnchorPos(entryEndPos, animationTime).SetUpdate(true);
    }

    private void DisablePauseMenu()
    {
        if (ableToPlaySFX)
            soundEffect.Play(audioSource);
            
        StartCoroutine(TweenThenDeactivate());
    }

    private IEnumerator TweenThenDeactivate()
    {
        Tweener tween = rectTransform.DOAnchorPos(exitEndPos, animationTime).SetUpdate(true);
        yield return tween.WaitForCompletion();

        pauseMenu.SetActive(false);
    }
}
