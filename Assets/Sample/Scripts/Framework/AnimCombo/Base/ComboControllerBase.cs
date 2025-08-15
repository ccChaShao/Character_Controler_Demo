using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Cysharp.Threading.Tasks;

public class ComboControllerBase : AnimBehaviourBase
{
    [Title("Combo")]
    [SerializeField, LabelText("招式列表资源")] private SOComboList m_comboList;

    [SerializeField, LabelText("预输入开启")] private bool m_enablePreInput;

    [SerializeField, LabelText("预输入获取")] private bool m_getPreInput;

    // 招式下标
    private int m_comboIndex;
    public int comboIndex
    {
        get => m_comboIndex;
        set => m_comboIndex = value;
    }

    // 招式进入
    private bool m_canExcuteCombo = true;
    public bool canExcuteCombo
    {
        get => m_canExcuteCombo;
        set => m_canExcuteCombo = value;
    }
    private CancellationTokenSource m_comboExcuteCdCts;
    
    // 招式推进
    private bool m_canMoveNextCombo = true;
    public bool canMoveNextCombo
    {
        get => m_canMoveNextCombo;
        set => m_canMoveNextCombo = value;
    }

    // 招式间隔
    private CancellationTokenSource m_gapComboCdCts;

    protected virtual void Update() { }

    protected virtual void OnEnable() {  }

    protected virtual void OnDisable()
    {
        ClearComboGapDelay();
        ClearComboExcuteDelay();
    }

    #region 功能函数

    protected void TryExcuteCombo()
    {
        // 招式CD（只有第一次进来需要判断）
        if (comboIndex <= 0 && !m_canExcuteCombo)
        {
            return;
        }
        // 招式内置CD
        if (!m_canMoveNextCombo)
        {
            return;
        }
        // 招式执行（动画进入）
        var config = m_comboList.TryGetComboConfig(m_comboIndex);
        if (config)
        {
            animator.CrossFadeInFixedTime(config.clipName, 0.155f, 0, 0);
        }
    }

    public void ClearCombo()
    {
        m_canExcuteCombo = true;        // 招式状态恢复
        m_comboIndex = 0;            // 下标重置
    }

    public void TrySetPreInput(bool value)
    {
        m_getPreInput = value && m_enablePreInput;
    }

    #endregion

    #region 定时器

    private async UniTask EnterComboGapDelay(int msDelay)
    {
        ClearComboGapDelay();
        m_gapComboCdCts = new();
        await UniTask.Delay(msDelay, cancellationToken: m_gapComboCdCts.Token);
    }

    private void ClearComboGapDelay()
    {
        m_gapComboCdCts?.Cancel();
        m_gapComboCdCts?.Dispose();
        m_gapComboCdCts = null;
    }

    private async UniTask EnterComboExcuteDelay(int msDelay)
    {
        ClearComboExcuteDelay();
        m_canExcuteCombo = false;
        m_comboExcuteCdCts = new();
        await UniTask.Delay(msDelay, cancellationToken: m_comboExcuteCdCts.Token);
        m_canExcuteCombo = true;
    }

    private void ClearComboExcuteDelay()
    {
        m_comboExcuteCdCts?.Cancel();
        m_comboExcuteCdCts?.Dispose();
        m_comboExcuteCdCts = null;
    }

    #endregion

    #region Anim Event

    public virtual void EnablePreInput()
    {
        m_enablePreInput = true;
    }

    #endregion

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var config = m_comboList.TryGetComboConfig(m_comboIndex);
        if (config)
        {
            // 连招正常进入
            if (stateInfo.IsName(config.clipName))
            {
                m_canMoveNextCombo = false;     // 关闭入口  
                m_enablePreInput = false;       // 关闭预输入
                m_getPreInput = false;          // 获取预输入
                // 进入招式CD（截断小数点）
                if (m_comboIndex <= 0)
                {
                    EnterComboExcuteDelay((int)m_comboList.cdDuring);
                }
                // 进入中断间隔CD（截断小数点）
                EnterComboGapDelay((int)m_comboList.gapDuring);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var config = m_comboList.TryGetComboConfig(m_comboIndex);
        if (config)
        {
            // 连招正常结束
            if (stateInfo.IsName(config.clipName))
            {
                m_canMoveNextCombo = true;          // 打开入口
                // 推进到下一个下标
                m_comboIndex = (m_comboIndex + 1) >= m_comboList.comboListCount ? 0 : m_comboIndex + 1;
                // 预输入
                if (m_getPreInput)
                {
                    TryExcuteCombo();
                }
            }
        }
    }
}
