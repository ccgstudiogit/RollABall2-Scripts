using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ResetSensitivityButton : MonoBehaviour
{
    public static event Action OnResetMouseSensitivityToDefault;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ResetMouseSensitivity);
    }

    public void ResetMouseSensitivity()
    {
        PlayerPrefs.SetFloat("mouseSensitivity", 0.5f);
        OnResetMouseSensitivityToDefault?.Invoke();
    }
}
