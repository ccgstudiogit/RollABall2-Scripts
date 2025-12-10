using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AdjustCameraSensitivity : MonoBehaviour
{
    [Header("Default Values")]
    [SerializeField] private float xAxisSpeed = 140f;
    [SerializeField] private float yAxisSpeed = 15f;

    [Header("Automatically Use Cinemachine's Sensitivity Settings")]
    [SerializeField] private bool enableAutoSensitivity;

    private CinemachineFreeLook cinemachineFreeLook;

    private void Awake()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();

        if (cinemachineFreeLook == null)
        {
            Debug.LogError("AdjustCameraSensitivity requires CinemachineFreeLook component to work. Disabling this script.");
            enabled = false;
        }
    }

    private void Start()
    {
        if (enableAutoSensitivity)
        {
            xAxisSpeed = cinemachineFreeLook.m_XAxis.m_MaxSpeed;
            yAxisSpeed = cinemachineFreeLook.m_YAxis.m_MaxSpeed;
        }
        else
        {
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = xAxisSpeed;
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = yAxisSpeed;
        }

        UpdateSensitivity();
    }

    private void OnEnable()
    {
        MouseSensitivitySlider.OnSensitivityChanged += UpdateSensitivity;
    }

    private void OnDisable()
    {
        MouseSensitivitySlider.OnSensitivityChanged -= UpdateSensitivity;
    }

    private void UpdateSensitivity()
    {
        if (SettingsManager.instance == null)
            return;

        cinemachineFreeLook.m_XAxis.m_MaxSpeed = xAxisSpeed * SettingsManager.instance.GetMouseSensitivityMultiplier();
        cinemachineFreeLook.m_YAxis.m_MaxSpeed = yAxisSpeed * SettingsManager.instance.GetMouseSensitivityMultiplier();
    }
}
