using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MoveAnimBehaviour : StateMachineBehaviour
{
    [ShowInInspector, ReadOnly] private MoveController m_moveController;
    [ShowInInspector, ReadOnly] private Animator m_animator;
    [ShowInInspector, ReadOnly] private Camera m_camera;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        m_camera = Camera.main;
        animator.applyRootMotion = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.applyRootMotion = true;
    }
}
