using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphicsDropdown : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    private void Start()
    {
        SetQualityValue();
    }

    public void ChangeGraphicsSettings(int qualityIndex)
    {
        if (SettingsManager.instance != null)
            SettingsManager.instance.SetQuality(qualityIndex);
    }

    private void SetQualityValue()
    {
        dropdown.value = PlayerPrefs.GetInt("quality", 2);
    }
}
