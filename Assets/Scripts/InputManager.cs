using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovmentValue { get; private set; }
    public event Action TargetEvent;
    public event Action CancelEvent;
    public event Action DodgeEvent;

    private bool isTargeting = false;
    Controls controls;

    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }
    private void OnDestroy()
    {
        controls.Player.Disable();
    }
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        DodgeEvent?.Invoke();
    }

    public void OnMovment(InputAction.CallbackContext context)
    {
        MovmentValue = context.ReadValue<Vector2>();
    }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (isTargeting)
        {
            CancelEvent?.Invoke();
        }
        else
        {
            TargetEvent?.Invoke();
        }

        isTargeting = !isTargeting;
    }
    public void ResetTargeting()
    {
        isTargeting = false;
    }
}
