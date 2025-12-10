using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResolutionDropdown : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    private void Start()
    {
        if (SettingsManager.instance != null)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(SettingsManager.instance.resolutionOptions);

            dropdown.value = SettingsManager.instance.currentResolutionIndex;
            dropdown.RefreshShownValue();
        }
    }

    public void ChangeResolution(int resolutionIndex)
    {
        if (SettingsManager.instance != null)
            SettingsManager.instance.SetResolution(resolutionIndex);
    }
}
