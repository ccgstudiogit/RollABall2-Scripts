using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class AnimationHelper : MonoBehaviour
{
    public static IEnumerator ZoomIn(RectTransform transform, float speed, UnityEvent OnEnd)
    {
        float time = 0;
        while (time < 1)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        transform.localScale = Vector3.one;

        OnEnd?.Invoke();
    }

    public static IEnumerator ZoomOut(RectTransform transform, float speed, UnityEvent OnEnd)
    {
        float time = 0;
        while (time < 1)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        transform.localScale = Vector3.zero;
        OnEnd?.Invoke();
    }

    public static IEnumerator FadeIn(CanvasGroup canvasGroup, float speed, UnityEvent OnEnd)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        float time = 0;
        while (time < 1)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        canvasGroup.alpha = 1;
        OnEnd?.Invoke();
    }

    public static IEnumerator FadeOut(CanvasGroup canvasGroup, float speed, UnityEvent OnEnd)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        float time = 0;
        while (time < 1)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        canvasGroup.alpha = 0;
        OnEnd?.Invoke();
    }

    public static void SlideIn(RectTransform transform, Vector2 entryEndPos, float speed, UnityEvent OnEnd)
    {
        transform.DOAnchorPos(entryEndPos, 1f / speed);
        OnEnd?.Invoke();
    }

    public static void SlideOut(RectTransform transform, Vector2 exitEndPos, float speed, UnityEvent OnEnd)
    {
        transform.DOAnchorPos(exitEndPos, 1f / speed);
        OnEnd?.Invoke();
    }
}
