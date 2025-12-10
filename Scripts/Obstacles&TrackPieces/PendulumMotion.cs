using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumMotion : MonoBehaviour
{
    [Header("Movement Values")]
    [SerializeField] private float rotationSpeed = 1.25f;
    [SerializeField] private float rotationAngle = 75f;
    Vector3 localRotation;
    private float rotationTime;
    private float currentRotationZ;

    [Header("Sound Effect")]
    [SerializeField] private SoundEffectSO soundEffect;
    [Tooltip("This game object's transform.localRotation.z (Euler angle) must be lower than this to play the sound effect")]
    [SerializeField] private float rotationPlayThreshold = 30f;
    [Tooltip("This game object's transform.localRotation.z (Euler angle) must be greater than this to reset the sound effect cooldown")]
    [SerializeField] private float rotationResetThreshold = 60f;
    private bool ableToPlaySFX = true;
    private bool hasPlayedSFX;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (soundEffect == null || audioSource == null)
        {
            Debug.LogWarning($"{name} is unable to play SFX due to missing soundEffect || missing audioSource.");
            ableToPlaySFX = false;
        }
    }

    private void Start()
    {
        localRotation = transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        rotationTime += Time.deltaTime;
        currentRotationZ = rotationAngle * Mathf.Sin(rotationTime * rotationSpeed);
        transform.localRotation = Quaternion.Euler(
            localRotation.x, 
            localRotation.y, 
            currentRotationZ
        );

        if (!ableToPlaySFX)
            return;

        // Adjust Z angle to handle negative rotations
        float normalizedRotationZ = transform.localRotation.eulerAngles.z;
        if (normalizedRotationZ > 180f)
            normalizedRotationZ -= 360f;

        if (!hasPlayedSFX && Mathf.Abs(normalizedRotationZ) < rotationPlayThreshold)
        {
            soundEffect.Play(audioSource);
            hasPlayedSFX = true;
        }

        if (hasPlayedSFX && Mathf.Abs(normalizedRotationZ) > rotationResetThreshold)
            hasPlayedSFX = false;
    }
}
