using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using UnityEditor.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class CharacterAnim : MonoBehaviour
{
    public AnimancerComponent AnimancerComponent;

    public AnimationClip Clip1;
    
    public AnimationClip Clip2;
    
    private InputService m_InputService;

    private void Awake()
    {
        m_InputService = InputService.Instance;
        m_InputService.inputSystem.Player.Attack.performed += OnAttackPerformed;
        m_InputService.inputSystem.Player.Attack.canceled += OnAttackCanceled;
    }

    private void OnEnable()
    {
        AnimancerComponent.Play(Clip1);
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        AnimancerState state = AnimancerComponent.Play(Clip2);
        state.Events(this).OnEnd = () =>
        {
            AnimancerComponent.Play(Clip1, 0.2f, FadeMode.FromStart);
        };
        Debug.Log("[OnAttackPerformed] : ---------------");
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("[OnAttackCanceled] : ---------------");
    }
}
