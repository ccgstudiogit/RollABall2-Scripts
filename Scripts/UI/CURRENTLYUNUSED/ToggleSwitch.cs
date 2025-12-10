using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
{
    [Header("Slider Setup")]
    [SerializeField, Range(0, 1f)] protected float sliderValue;
    private Slider slider;

    public bool currentValue { get; private set; }

    [Header("Animation")]
    [SerializeField, Range(0, 1f)] private float animationDuration = 0.25f;
    [SerializeField] private AnimationCurve slideEase = 
        AnimationCurve.EaseInOut(timeStart: 0, valueStart: 0, timeEnd: 1, valueEnd: 1);
    private Coroutine animationSliderCoroutine;

    [Header("Events")]
    [SerializeField] private UnityEvent OnToggleOn;
    [SerializeField] private UnityEvent OnToggleOff;
    protected Action transitionEffect;

    protected virtual void Awake()
    {
        SetupToggleComponents();
    }

    protected void OnValidate()
    {
        SetupToggleComponents();

        slider.value = sliderValue;
    }
    
    private void SetupToggleComponents()
    {
        if (slider != null)
            return;

            SetupSliderComponents();
    }

    private void SetupSliderComponents()
    {
        slider = GetComponent<Slider>();

        if (slider == null)
        {
            Debug.LogWarning($"No slider found on {name}");
            return;
        }

        slider.interactable = false;
        var sliderColors = slider.colors;
        sliderColors.disabledColor = Color.white;
        slider.colors = sliderColors;
        slider.transition = Selectable.Transition.None;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Toggle();
    }

    private void Toggle()
    {
        SetStateAndStartAnimation(!currentValue);
    }

    private void SetStateAndStartAnimation(bool state)
    {
        currentValue = state;

        if (currentValue)
            OnToggleOn?.Invoke();
        else
            OnToggleOff?.Invoke();

        if (animationSliderCoroutine != null)
            StopCoroutine(animationSliderCoroutine);

        animationSliderCoroutine = StartCoroutine(AnimateSlider());
    }

    private IEnumerator AnimateSlider()
    {
        float startValue = slider.value;
        float endValue = currentValue ? 1 : 0;
        
        float time = 0;
        if (animationDuration > 0)
        {
            while (time < animationDuration)
            {
                time += Time.deltaTime;

                float lerpFactor = slideEase.Evaluate(time / animationDuration);
                slider.value = sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);

                transitionEffect?.Invoke();

                yield return null;
            }
        }

        slider.value = endValue;
    }
}
