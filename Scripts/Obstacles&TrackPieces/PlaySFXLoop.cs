using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySFXLoop : MonoBehaviour
{
    [SerializeField] private SoundEffectSO soundEffect;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (soundEffect == null)
        {
            Debug.LogError($"{name}'s soundEffect null. Please assign a reference. Disabling this script.");
            enabled = false;
        }

        audioSource.loop = true;
    }

    private void Start()
    {
        soundEffect.Play(audioSource);
    }

    private void OnEnable()
    {
        GameController.OnPause += Mute;
        GameController.OnResume += UnMute;
    }

    private void OnDisable()
    {
        GameController.OnPause -= Mute;
        GameController.OnResume -= UnMute;
    }

    private void Mute()
    {
        audioSource.mute = true;
    }

    private void UnMute()
    {
        audioSource.mute = false;
    }
}
