using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySFXEveryXSeconds : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SoundEffectSO soundEffect;
    [SerializeField] private float playEveryXSecond = 2f;
    [SerializeField] private bool includeVariation;
    [SerializeField] private float variation = 1f;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (soundEffect == null)
        {
            Debug.LogError($"{name}'s soundEffect null. Please assign a reference. Disabling this script.");
            enabled = false;
        }
    }

    private void Start()
    {
        StartCoroutine(PlaySFX());
    }

    private IEnumerator PlaySFX()
    {
        while (enabled)
        {
            float waitTime = playEveryXSecond;

            if (includeVariation)
                waitTime = Random.Range(playEveryXSecond - variation, playEveryXSecond + variation);

            yield return new WaitForSeconds(waitTime);

            soundEffect.Play(audioSource);
        }
    }
}
