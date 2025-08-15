using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimBehaviourBase : MonoBehaviour
{
    protected Animator animator;
    
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){ }

    public virtual void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
