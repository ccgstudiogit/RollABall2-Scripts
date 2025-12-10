using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance { get; private set; }

    [HideInInspector] public List<string> resolutionOptions = new List<string>();
    public int currentResolutionIndex { get; private set; }

    private List<Resolution> filteredResolutions = new List<Resolution>();
    private Resolution[] resolutions;
    private double currentRefreshRate;

    [HideInInspector] public float mouseSensitivityMultiplierMultiplier = 2; // The multiplier for the mouse sensitivity multiplier
    private float mouseSensitivityMultiplier;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            LoadChosenQuality();
            LoadCurrentMouseSensitivity();

            currentResolutionIndex = 0;
            currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

            GetResolutions();
        }
        else
            Destroy(gameObject);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("quality", qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("fullscreen", isFullscreen == true ? 1 : 0); // 0 == not fullscreen, 1 == fullscreen
    }

    public void SetResolution(int index)
    {
        Resolution resolution = filteredResolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        currentResolutionIndex = index;
    }

    public void SetMouseSensitivity(float amount)
    {
        mouseSensitivityMultiplier = amount;
    }

    public float GetMouseSensitivityMultiplier()
    {
        return mouseSensitivityMultiplier;
    }

    private void GetResolutions()
    {
        resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate)
                filteredResolutions.Add(resolutions[i]);
        }

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string option = filteredResolutions[i].width + " x " + filteredResolutions[i].height;

            resolutionOptions.Add(option);

            if (filteredResolutions[i].height == Screen.height && filteredResolutions[i].width == Screen.width)
                currentResolutionIndex = i;
        }
    }

    private void LoadChosenQuality()
    {
        int qualityIndex = PlayerPrefs.GetInt("quality", 2); // 0 == Low, 1 == Medium, 2 == High (High is default)
        SetQuality(qualityIndex);
    }

    private void LoadCurrentMouseSensitivity()
    {
        mouseSensitivityMultiplier = PlayerPrefs.GetFloat("mouseSensitivity", 0.5f) * mouseSensitivityMultiplierMultiplier;
    }
}
