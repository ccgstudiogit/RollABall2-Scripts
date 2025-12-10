using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounceOffThisObject : MonoBehaviour
{
    [Header("References")]
    [Tooltip("This reference is for objects that may need to calculate the force direction " +
        "from a better transform.position than the original object's transform.position")]
    [SerializeField] private GameObject objectCenter;
    private Vector3 direction;

    [Header("Collision Values")]
    [SerializeField] private float collisionForce = 225f;

    [Header("Sound Effect")]
    [SerializeField] private SoundEffectSO soundEffect;
    private AudioSource audioSource;
    private bool ableToPlaySFX = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (soundEffect == null || audioSource == null)
        {
            Debug.LogWarning($"{name} is unable to play SFX due to missing soundEffect || missing audioSource.");
            ableToPlaySFX = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (objectCenter != null)
                direction = (collision.gameObject.transform.position - objectCenter.transform.position).normalized;
            else
                direction = (collision.gameObject.transform.position - transform.position).normalized;

            if (collision.gameObject.TryGetComponent(out Rigidbody playerRb))
                playerRb.AddForce(direction * collisionForce, ForceMode.Impulse);

            if (ableToPlaySFX)
                soundEffect.Play(audioSource);
        }
    }
}
