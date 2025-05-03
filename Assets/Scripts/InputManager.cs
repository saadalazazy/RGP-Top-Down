using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovmentValue { get; private set; }

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

    public void OnMovment(InputAction.CallbackContext context)
    {
        MovmentValue = context.ReadValue<Vector2>();
    }
}
