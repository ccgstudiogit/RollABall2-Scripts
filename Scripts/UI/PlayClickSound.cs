using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayClickSound : MonoBehaviour
{
    [SerializeField] private SoundEffectSO clickSound;
    [SerializeField] private AudioSource audioSource;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        if (audioSource == null)
            Debug.LogWarning($"{name}'s optional audioSource null.");
            
        if (clickSound == null)
        {
            Debug.LogError($"{name}'s clickSound null. Please assign a reference. Disabling this script.");
            enabled = false;
        }

        button.onClick.AddListener(PlaySound);
    }

    public void PlaySound()
    {
        if (audioSource != null)
            clickSound.Play(audioSource);
        else
            clickSound.Play();
    }
}
