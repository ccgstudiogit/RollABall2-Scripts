using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSideToSide : MonoBehaviour
{
    [Header("Movement Values")]
    [Tooltip("The starting position for this game object")]
    [SerializeField] private Vector3 positionOne = Vector3.zero;
    [Tooltip("Enable this setting to automatically set positionOne to be this object's current position")]
    [SerializeField] private bool positionOneEqualsCurrentPos;
    [SerializeField] private Vector3 positionTwo = Vector3.zero;
    private Vector3 targetPos;
    [SerializeField] private float movementDuration = 1f;
    [SerializeField] private float secondsBetweenMovement = 2f;
    [SerializeField] private float delay = 0;
    private bool moving;

    [Header("Sound Effect")]
    [Tooltip("The sound effect that will play once this game object stops moving")]
    [SerializeField] private SoundEffectSO stopSoundEffect;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogWarning($"{name} does not have an audio source. Unable to play sound effects.");
    }

    private void Start()
    {
        if (positionOneEqualsCurrentPos)
            positionOne = transform.position;

        targetPos = positionTwo;

        StartCoroutine(MovementHandler());
    }

    private IEnumerator MovementHandler()
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        while (enabled)
        {
            if (!moving)
                StartCoroutine(Move());

            yield return new WaitForSeconds(secondsBetweenMovement + movementDuration);
        }
    }

    private IEnumerator Move()
    {
        moving = true;
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < movementDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / movementDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        targetPos = targetPos == positionOne ? positionTwo : positionOne;

        if (stopSoundEffect != null && audioSource != null)
            stopSoundEffect.Play(audioSource);

        moving = false;
    }
}
