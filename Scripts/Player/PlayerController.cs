using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform mainCam;
    private InputActions inputActions;
    private Rigidbody rb;
    private SphereCollider col;

    [Header("Movement Values")]
    [SerializeField] private float moveSpeed = 425f;
    [Tooltip("The player can move any object that has this layer mask")]
    [SerializeField] private LayerMask moveableLayer;
    private Vector2 moveInput;
    private Vector3 moveDirection;
    private bool isGrounded = false;
    private float checkRadius = 0; // Ground check radius

    [Header("Angular Dampening")]
    [Tooltip("Threshold for stopping rotation")]
    [SerializeField] private float angularDragThreshold = 0.05f;
    [Tooltip("Once the velocity reaches this threshold, angular dampening begins")]
    [SerializeField] private float minVelThreshold = 0.1f;
    [SerializeField] private float angularDampingDuration = 1f;
    private Coroutine angularDampingCoroutine;

    private void Awake()
    {
        inputActions = new InputActions();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();

        if (col != null)
            checkRadius = col.radius * 1.1f; // Set the ground check radius to be slightly larger than the player radius

        if (mainCam == null)
        {
            Debug.LogWarning($"{name}'s mainCam reference null. Please assign a reference.");
            enabled = false;
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();

        VictoryZone.OnPlayerWin += () => inputActions.Disable();
    }

    private void OnDisable()
    {
        inputActions.Disable();

        VictoryZone.OnPlayerWin -= () => inputActions.Disable();
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Movement();
        HandleAngularDamping();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ITriggerable triggerable))
            triggerable.Trigger();
    }

    private void GetInput()
    {
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
    }

    private void GroundCheck()
    {
        Vector3 pos = transform.position;
        isGrounded = Physics.CheckSphere(pos, checkRadius, moveableLayer);
    }

    private void Movement()
    {
        // Calculate movement direction and magnitude
        moveDirection = (moveInput.y * mainCam.forward) + (moveInput.x * mainCam.right);

        // Add force to rigidbody if player is grounded
        if (isGrounded)
            rb.AddForce(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleAngularDamping()
    {
        // Check if ball is nearly stationary
        if (rb.velocity.magnitude < minVelThreshold && rb.angularVelocity.magnitude > angularDragThreshold)
        {
            if (angularDampingCoroutine == null)
                angularDampingCoroutine = StartCoroutine(DampenAngularVelocity());
        }
    }

    private IEnumerator DampenAngularVelocity()
    {
        float elapsedTime = 0f;
        Vector3 initialAngularVelocity = rb.angularVelocity;

        while (elapsedTime < angularDampingDuration)
        {
            // Stops coroutine if movement is detected
            if (moveInput != Vector2.zero && angularDampingCoroutine != null)
            {
                StopCoroutine(angularDampingCoroutine);
                angularDampingCoroutine = null;
            }

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / angularDampingDuration;

            rb.angularVelocity = Vector3.Lerp(initialAngularVelocity, Vector3.zero, t);

            yield return null;
        }

        rb.angularVelocity = Vector3.zero;
        angularDampingCoroutine = null;
    }
}
