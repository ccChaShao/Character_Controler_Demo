using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using InputAction = UnityEngine.InputSystem.InputAction;

public class PlayerComboController : ComboControllerBase
{
    private bool m_isAttackDirty;
    
    private InputService m_inputService;

    protected override void Awake()
    {
        base.Awake();
        m_inputService = InputService.Instance;
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        m_inputService.onAttackPerformed.AddListener(OnAttackPerformed);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        m_inputService.onAttackPerformed.RemoveListener(OnAttackPerformed);
    }

    protected override void Update()
    {
        base.Update();
        CheckNormalAttackInput();
    }

    private void CheckNormalAttackInput()
    {
        if (m_isAttackDirty)
        {
            TrySetPreInput(true);       // 预输入
            TryExcuteCombo();           // 基类只管尝试进入；
        }
        m_isAttackDirty = false;            // 清掉
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        m_isAttackDirty = true;
    }
}
