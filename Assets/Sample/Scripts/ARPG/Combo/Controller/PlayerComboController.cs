using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using InputAction = UnityEngine.InputSystem.InputAction;

public class PlayerComboController : ComboControllerBase
{
    private bool m_isAttackDirty;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        InputService.Instance.onAttackPerformed.AddListener(OnAttackPerformed);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InputService.Instance.onAttackPerformed.RemoveListener(OnAttackPerformed);
    }

    protected override void Update()
    {
        base.Update();
        CheckAttackInput();
    }

    private void CheckAttackInput()
    {
        Debug.Log("charsiew : [CheckInput] : ----------------" + m_isAttackDirty);
        if (m_isAttackDirty)
        {
            if (m_canExcuteCombo)
            {
                ExcuteCombo();          // 内部只关系进入combo，至于combo执行什么，由底层管理
            }
        }

        m_isAttackDirty = false;            // 清掉
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        m_isAttackDirty = true;
    }
}
