using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AttackAnimBehaviour : StateMachineBehaviour
{
    [ShowInInspector, ReadOnly] private MoveController m_moveController;
    
    [ShowInInspector, ReadOnly] private Animator m_animator;

    public MoveController moveController
    {
        get
        {
            if (m_animator && !m_moveController)
            {
                m_moveController = m_animator.GetComponent<MoveController>();
            }

            return m_moveController;
        }
    }
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        m_animator = animator;
        if (stateInfo.IsTag("attack"))
        {
            moveController.enableMove = false;
            moveController.ClearMovement();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        m_animator = animator;
        if (stateInfo.IsTag("attack"))
        {
            moveController.enableMove = true;
        }
    }
}
