using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] private AudioMixer mixer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (mixer != null)
            LoadAudioPlayerPrefs();
        else
            Debug.LogError("AudioManager's mixer reference null. Please assign a reference.");
    }
    
    private void LoadAudioPlayerPrefs()
    {
        float musicVol = PlayerPrefs.GetFloat("musicVol", 1f);
        float sfxVol = PlayerPrefs.GetFloat("sfxVol", 1f);

        SetMusicVolume(musicVol);
        SetSFXVolume(sfxVol);
    }

    public void SetMusicVolume(float sliderValue)
    {
        // Converts to logarithm to the base of 10. This is done because it takes the slider value 0.0001 to 1
        // and turns it into a value between -80 and 0 but on a logarithmic scale
        mixer.SetFloat("musicVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("musicVol", sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        mixer.SetFloat("sfxVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("sfxVol", sliderValue);
    }
}
