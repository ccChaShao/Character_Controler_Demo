using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent), typeof(Animator))]
public class PlayerController : CharacterBase
{
    [Header("Config")]
    public Transform cameraTransform;
    
    public AnimancerComponent animancer { get; private set; }
    public InputService inputService { get; private set; }
    public TimerService timerService { get; private set; }

    private void Awake()
    {
        inputService = InputService.Instance;
        timerService = TimerService.Instance;
        animancer = GetComponent<AnimancerComponent>();
    }
}
