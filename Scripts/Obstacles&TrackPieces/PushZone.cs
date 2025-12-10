using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushZone : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference this game object here")]
    [SerializeField] private GameObject pushOrigin;
    private BoxCollider col; // Must be a trigger box collider

    [Header("Strength Values")]
    [Tooltip("When making first contact, this push strength is registered immediately")]
    [SerializeField] private float initialPushStrength = 10f;
    [SerializeField] private float pushStrength = 125f;
    private Vector3 direction = Vector3.zero;
    [Tooltip("If enabled, while the player is within the box collider the force strength will increase over time")]
    [SerializeField] private bool increaseStrengthOverTime = true;
    [SerializeField] private float increaseOverTimeMultiplier = 1.1f, increaseEveryXSeconds = 0.33f;
    private float strengthMultiplier;
    private Coroutine increaseStrengthCoroutine = null;
    private bool playerInPushZone;

    private void Awake()
    {
        if (pushOrigin != null)
            direction = pushOrigin.transform.forward;
        else
            Debug.LogWarning($"{name}'s pushOrigin reference null. Unable to calculate push direction.");

        col = GetComponent<BoxCollider>();
        if (col == null)
        {
            Debug.LogWarning($"{name} does not have a box collider. PushZone disabled.");
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Rigidbody playerRb))
        {   
            playerInPushZone = true;
            strengthMultiplier = 1f;

            if (increaseStrengthOverTime && increaseStrengthCoroutine == null)
                StartCoroutine(IncreaseStrength());

            playerRb.AddForce(direction * initialPushStrength, ForceMode.Impulse);
        }   
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out Rigidbody playerRb))
            playerRb.AddForce(direction * pushStrength * strengthMultiplier);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (increaseStrengthCoroutine != null)
                StopCoroutine(increaseStrengthCoroutine);

            playerInPushZone = false;
            increaseStrengthCoroutine = null;
        }
    }

    private IEnumerator IncreaseStrength()
    {
        while (playerInPushZone)
        {
            strengthMultiplier *= increaseOverTimeMultiplier;
            yield return new WaitForSeconds(increaseEveryXSeconds);
        }
    }
}
