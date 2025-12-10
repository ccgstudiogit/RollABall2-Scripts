using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFadeInOut : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float timeToFade = 1f;
    private bool fadeIn;
    private bool fadeOut;

    public void FadeIn()
    {
        fadeIn = true;
        StartCoroutine(FadeInRoutine());
    }

    public void FadeOut()
    {
        fadeOut = true;
        StartCoroutine(FadeOutRoutine());
    }

    public float GetTimeToFade()
    {
        return timeToFade;
    }

    private IEnumerator FadeOutRoutine()
    {
        // Scene view fading out
        while (fadeOut)
        {
            if (canvasGroup.alpha >= 0)
            {
                if (Time.timeScale == 0)
                    canvasGroup.alpha += timeToFade * Time.unscaledDeltaTime;
                else
                    canvasGroup.alpha += timeToFade * Time.deltaTime;

                if (canvasGroup.alpha >= 1)
                {
                    canvasGroup.alpha = 1;
                    fadeOut = false;
                }
            }

            yield return null;
        }
    }

    private IEnumerator FadeInRoutine()
    {
        // Scene view fading in
        while (fadeIn)
        {
            if (canvasGroup.alpha <= 1)
            {
                if (Time.timeScale == 0)
                    canvasGroup.alpha -= timeToFade * Time.unscaledDeltaTime;
                else
                    canvasGroup.alpha -= timeToFade * Time.deltaTime;

                if (canvasGroup.alpha <= 0)
                {
                    canvasGroup.alpha = 0;
                    fadeIn = false;
                }
            }

            yield return null;
        }
    }
}
