using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Serialization;

public class BaseLayerBehaviourScript : StateMachineBehaviour
{
    [LabelText("角色"), Required("缺少挂载点！")] 
    public GameObject character;
    
    [LabelText("AnimBehaviourBase"), ShowInInspector, ReadOnly] 
    private AnimBehaviourBase[] m_animBehaviourBaseList;

    private void Awake()
    {
        if (character)
        {
            m_animBehaviourBaseList = character.GetComponents<AnimBehaviourBase>();
        }
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (m_animBehaviourBaseList != null)
        {
            for (int i = 0; i < m_animBehaviourBaseList.Length; i++)
            {
                var behaviour = m_animBehaviourBaseList[i];
                behaviour.OnStateEnter(animator, stateInfo, layerIndex);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        if (m_animBehaviourBaseList != null)
        {
            for (int i = 0; i < m_animBehaviourBaseList.Length; i++)
            {
                var behaviour = m_animBehaviourBaseList[i];
                behaviour.OnStateExit(animator, stateInfo, layerIndex);
            }
        }
    }
}
