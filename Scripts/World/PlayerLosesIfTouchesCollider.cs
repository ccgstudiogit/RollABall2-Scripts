using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerLosesIfTouchesCollider : MonoBehaviour, ITriggerable
{
    public static event Action OnPlayerLose;
    public static event Action OnPlayerFall; // This exists so that the death animation plays if the player wins, then falls

    [SerializeField] private SoundEffectSO fallSoundEffect;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogWarning($"{name}'s audioSource null. Unable to play fall sound effect on player fall.");
    }

    public void Trigger()
    {
        if (GameController.instance != null)
        {
            if (GameController.instance.gameActive)
                OnPlayerLose?.Invoke();
            else if (!GameController.instance.gameActive)
                OnPlayerFall?.Invoke();
        }
        else
            OnPlayerLose?.Invoke();
        
        if (fallSoundEffect != null && audioSource != null)
            fallSoundEffect.Play(audioSource);
    }
}
