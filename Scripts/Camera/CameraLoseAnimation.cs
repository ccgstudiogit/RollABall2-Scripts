using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraLoseAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject lookAtTarget;
    [SerializeField] private float lookAtDuration = 7f;

    private void Start()
    {
        if (lookAtTarget == null)
            Debug.LogWarning($"{name}'s lookAtTarget null. CameraLoseAnimation will not follow a target.");

        PlayerLosesIfTouchesCollider.OnPlayerLose += PlayerFell;
        PlayerLosesIfTouchesCollider.OnPlayerFall += PlayerFell;
    }

    private void OnDisable()
    {
        PlayerLosesIfTouchesCollider.OnPlayerLose -= PlayerFell;
        PlayerLosesIfTouchesCollider.OnPlayerFall -= PlayerFell;
    }

    private void PlayerFell()
    {
        // Disable player controls to camera
        if (TryGetComponent(out CinemachineBrain cinemachineBrain))
            cinemachineBrain.enabled = false;

        if (lookAtTarget != null)
            StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        float elapsedTime = 0f;

        while (elapsedTime <= lookAtDuration)
        {
            if (lookAtTarget != null)
                transform.LookAt(lookAtTarget.transform);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
