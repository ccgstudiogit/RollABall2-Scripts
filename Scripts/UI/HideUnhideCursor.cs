using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUnhideCursor : MonoBehaviour
{
    [SerializeField] private bool lockOnStart = true;

    private void Start()
    {
        if (lockOnStart)
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        GameController.OnPause += UnlockCursor;
        GameController.OnResume += LockCursor;

        PlayerLosesIfTouchesCollider.OnPlayerLose += UnlockCursor;
        VictoryZone.OnPlayerWin += UnlockCursor;
    }

    private void OnDisable()
    {
        GameController.OnPause -= UnlockCursor;
        GameController.OnResume -= LockCursor;

        PlayerLosesIfTouchesCollider.OnPlayerLose -= UnlockCursor;
        VictoryZone.OnPlayerWin -= UnlockCursor;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
