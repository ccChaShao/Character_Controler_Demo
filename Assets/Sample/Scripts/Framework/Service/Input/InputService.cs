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
    
    public UnityEvent<InputAction.CallbackContext> onMovePerformed = new ();
    
    public UnityEvent<InputAction.CallbackContext> onMoveCanceled = new ();
    
    public UnityEvent<InputAction.CallbackContext> onJumpPerformed = new ();


    public Vector2 scrollVal => inputSystem.Player.Scroll.ReadValue<Vector2>();

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

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        onMovePerformed?.Invoke(context);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        onMoveCanceled?.Invoke(context);
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        onJumpPerformed?.Invoke(context);
    }

    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Player.Attack.performed += OnAttackPerformed;
        inputSystem.Player.Shift.performed += OnShiftPerformed;
        inputSystem.Player.Scroll.performed += OnScrollPerformed;
        inputSystem.Player.Move.performed += OnMovePerformed;
        inputSystem.Player.Move.canceled += OnMoveCanceled;
        inputSystem.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        inputSystem.Disable();
        inputSystem.Player.Attack.performed -= OnAttackPerformed;
        inputSystem.Player.Shift.performed -= OnShiftPerformed;
        inputSystem.Player.Scroll.performed -= OnScrollPerformed;
        inputSystem.Player.Move.performed -= OnMovePerformed;
        inputSystem.Player.Move.canceled -= OnMoveCanceled;
        inputSystem.Player.Jump.performed -= OnJumpPerformed;
    }
}
