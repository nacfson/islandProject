using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "SO/Input/Input_PlayerInput")]
public class Input_PlayerInput : ScriptableObject,PlayerInput.IControlActions
{
    private PlayerInput _playerInput;


    public event Action<Vector2> OnMovementKeyPress;
    public event Action OnInventoryKeyPress;
    public event Action OnInteractKeyPress;
    public event Action OnActionKeyPress;
    public event Action OnAnyKeyPress;
    public event Action<bool> OnRunKeyPress;

    private void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInput();
            _playerInput.Control.SetCallbacks(this);
        }
        
        _playerInput.Control.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        OnMovementKeyPress?.Invoke(value);
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnInventoryKeyPress?.Invoke();
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnInteractKeyPress?.Invoke();
        }
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnActionKeyPress?.Invoke();
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnRunKeyPress?.Invoke(true);
        }
        else
        {
            OnRunKeyPress?.Invoke(false);
        }
    }

    public void OnAny(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnAnyKeyPress?.Invoke();
    }
}

