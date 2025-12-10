using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioType audioType;
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        float volume = 1f;

        switch (audioType)
        {
            case AudioType.MUSIC:
                volume  = PlayerPrefs.GetFloat("musicVol", 1f);
                break;
            case AudioType.SFX:
                volume = PlayerPrefs.GetFloat("sfxVol", 1f);
                break;
        }

        slider.value = volume;
        slider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float volume)
    {
        if (AudioManager.instance != null)
        {
            switch (audioType)
            {
                case AudioType.MUSIC:
                    AudioManager.instance.SetMusicVolume(volume);
                    break;
                case AudioType.SFX:
                    AudioManager.instance.SetSFXVolume(volume);
                    break;
            }
        }
    }
}
