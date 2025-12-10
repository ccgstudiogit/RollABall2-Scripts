using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CustomCinemachineInputProvider : CinemachineInputProvider
{
    [SerializeField] private InputActionReference lookButton;

    private void OnEnable()
    {
        if (lookButton != null && lookButton.action != null)
            lookButton.action.Enable();
    }

    protected override void OnDisable()
    {
        if (lookButton != null && lookButton.action != null)
            lookButton.action.Disable();

        base.OnDisable();
    }

    public override float GetAxisValue(int axis)
    {
        if (enabled)
        {
            if (lookButton != null && lookButton.action != null)
            {
                if (lookButton.action.ReadValue<float>() == 0)
                    return 0f;
            }

            return base.GetAxisValue(axis);
        }
    
        return 0f;
    }
}
