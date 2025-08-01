using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool isShift => inputSystem.Player.Shift.ReadValue<float>() != 0;

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

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }
}
