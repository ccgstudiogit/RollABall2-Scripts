using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MouseSensitivitySlider : MonoBehaviour
{
    public static event Action OnSensitivityChanged;

    private Slider slider;
    private float amount;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        amount = PlayerPrefs.GetFloat("mouseSensitivity", 0.5f); // 0.5 is default. Will be multiplied by 2 so 0.5 will be 1
        slider.value = amount;
        slider.onValueChanged.AddListener(SetSensitivity);
    }

    private void OnEnable()
    {
        ResetSensitivityButton.OnResetMouseSensitivityToDefault += UpdateSlider;
    }

    private void OnDisable()
    {
        ResetSensitivityButton.OnResetMouseSensitivityToDefault -= UpdateSlider;
    }

    private void SetSensitivity(float amount)
    {
        if (SettingsManager.instance != null)
        {
            PlayerPrefs.SetFloat("mouseSensitivity", amount);
            SettingsManager.instance.SetMouseSensitivity(amount * SettingsManager.instance.mouseSensitivityMultiplierMultiplier);
            OnSensitivityChanged?.Invoke();
        }
    }

    private void UpdateSlider()
    {
        amount = PlayerPrefs.GetFloat("mouseSensitivity", 0.5f);
        slider.value = amount;
    }
}
