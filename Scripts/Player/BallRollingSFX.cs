using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Rigidbody))]
public class BallRollingSFX : MonoBehaviour
{
    [Tooltip("The rolling SFX will only play if the ball is rolling on an object that has this layer")]
    [SerializeField] private LayerMask groundMask;
    [Tooltip("The minimum speed required for the sound effect to play")]
    [SerializeField] private float minSpeed = 0.1f;

    private Rigidbody rb;
    private AudioSource ballSoundSource;
    private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ballSoundSource = GetComponent<AudioSource>();
        speed = 0;
    }

    private void Update()
    {
        ballSoundSource.volume = 2;
        ballSoundSource.pitch = Mathf.Clamp01(speed);

        if (ballSoundSource.isPlaying == false && speed >= minSpeed)
            ballSoundSource.Play();
        else if (ballSoundSource.isPlaying == true && speed < minSpeed)
            ballSoundSource.Stop();
    }

    private void FixedUpdate()
    {
        speed = rb.velocity.magnitude;
    }
    /*
    private void OnCollisionStay(Collision collision)
    {
        if (ballSoundSource.isPlaying == false && speed >= minSpeed && collision.gameObject.layer == groundMask)
        {
            ballSoundSource.Play();
            Debug.Log("Playing");
        }
        else if (ballSoundSource.isPlaying == true && speed < minSpeed && collision.gameObject.layer == groundMask)
            ballSoundSource.Pause();
    }

    private void OnCollisionExit(Collision collision)
    {
        if (ballSoundSource.isPlaying == true && collision.gameObject.layer == groundMask)
            ballSoundSource.Pause();
    }
    */
}
