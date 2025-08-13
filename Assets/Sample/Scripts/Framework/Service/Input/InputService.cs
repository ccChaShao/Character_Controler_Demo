using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using InputAction = UnityEngine.InputSystem.InputAction;

public class InputService : MonoSingleton<InputService>
{
    private InputSystem_Actions _inputSystem;

    public InputSystem_Actions inputSystem
    {
        get
        {
            if (_inputSystem == null)
            {
                _inputSystem = new InputSystem_Actions();
                _inputSystem.Enable();
            }

            return _inputSystem;
        }
    }

    public UnityEvent<InputAction.CallbackContext> onAttackPerformed = new ();
    
    public UnityEvent<InputAction.CallbackContext> onShiftPerformed = new ();
    
    public UnityEvent<InputAction.CallbackContext> onScrollPerformed = new ();


    public Vector2 scrollVal => inputSystem.Player.Scroll.ReadValue<Vector2>();

    public Vector2 moveVal
    {
        get
        {
            Vector2 moveV2 = inputSystem.Player.Move.ReadValue<Vector2>();
            if (moveV2.x > 0)
            {
                moveV2.x = 1;
            }
            else if (moveV2.x < 0)
            {
                moveV2.x = -1;
            }

            if (moveV2.y > 0)
            {
                moveV2.y = 1;
            }
            else if (moveV2.y < 0)
            {
                moveV2.y = -1;
            }

            return moveV2;
        }
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        onAttackPerformed?.Invoke(context);
    }

    private void OnShiftPerformed(InputAction.CallbackContext context)
    {
        onShiftPerformed?.Invoke(context);
    }

    private void OnScrollPerformed(InputAction.CallbackContext context)
    {
        onScrollPerformed?.Invoke(context);
    }

    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Player.Attack.performed += OnAttackPerformed;
        inputSystem.Player.Shift.performed += OnShiftPerformed;
        inputSystem.Player.Scroll.performed += OnScrollPerformed;
    }

    private void OnDisable()
    {
        inputSystem.Disable();
        inputSystem.Player.Attack.performed -= OnAttackPerformed;
        inputSystem.Player.Shift.performed -= OnShiftPerformed;
        inputSystem.Player.Scroll.performed -= OnScrollPerformed;
    }
}
