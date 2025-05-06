using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Controls.IPlayerActions
{
    public Vector2 MovmentValue { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsAming { get; private set; }
    public bool IsMouseUse { get; private set; } = false;

    public event Action DodgeEvent;

    Controls controls;

    private void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();

        InputSystem.onActionChange += OnActionChange;
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
        InputSystem.onActionChange -= OnActionChange;
    }

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            var action = (InputAction)obj;
            var control = action.activeControl;
            if (control != null)
            {
                IsMouseUse = control.device switch
                {
                    Mouse => true,
                    _ => false
                };
            }
        }
    }

    public void OnMovment(InputAction.CallbackContext context)
    {
        MovmentValue = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsAttacking = true;
        }
        else if (context.canceled)
        {
            IsAttacking = false;
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsAming = true;
        }
        else if (context.canceled)
        {
            IsAming = false;
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        DodgeEvent?.Invoke();
    }
}