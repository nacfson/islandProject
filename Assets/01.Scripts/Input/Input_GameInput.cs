using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "SO/Input/Input_GameInput")]
public class Input_GameInput : ScriptableObject,GameInput.IManagerActions
{
    private GameInput _gameInput;

    public event Action OnAnyKeyPress;
    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
            _gameInput.Manager.SetCallbacks(this);
        }
        
        _gameInput.Manager.Enable();
    }


    public void OnAny(InputAction.CallbackContext context)
    {
        if(context.performed)
            OnAnyKeyPress?.Invoke();
    }
}