using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZAxisEveryXSeconds : MonoBehaviour
{
    [Header("Movement Values")]
    [SerializeField] private float rotationSpeed = 125f;
    [Tooltip("The time in seconds between each rotation")]
    [SerializeField] private float rotateEveryXSeconds = 3f;
    [SerializeField] private float delay = 0;
    private bool rotating;
    private float rotationAmount = 180f;

    [Header("Sound Effects")]
    [Tooltip("The sound effect that will play once this object starts rotating")]
    [SerializeField] private SoundEffectSO beginRotationSE;
    [Tooltip("The sound effect that will play once this object finishes its rotation")]
    [SerializeField] private SoundEffectSO endRotationSE;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogWarning($"{name} does not have an audio source. Unable to play sound effects.");
    }

    private void Start()
    {
        StartCoroutine(RotateHandler());
    }

    private IEnumerator RotateHandler()
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        while (enabled)
        {
            if (!rotating)
                StartCoroutine(Rotate());

            yield return new WaitForSeconds(rotateEveryXSeconds);
        }
    }

    private IEnumerator Rotate()
    {
        rotating = true;
        float rotatedAmount = 0;

        if (audioSource != null && beginRotationSE != null)
            beginRotationSE.Play(audioSource);
        
        while (rotatedAmount < rotationAmount)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            rotationStep = Mathf.Min(rotationStep, rotationAmount - rotatedAmount);

            transform.Rotate(0, 0, rotationStep);

            rotatedAmount += rotationStep;

            yield return null;
        }

        Vector3 localRotation = transform.localEulerAngles;
        localRotation.z = Mathf.Round(localRotation.z);
        transform.localEulerAngles = localRotation;

        if (audioSource != null && endRotationSE != null)
            endRotationSE.Play(audioSource);

        rotating = false;
    }
}
