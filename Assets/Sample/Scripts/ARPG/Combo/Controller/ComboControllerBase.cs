using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Cysharp.Threading.Tasks;
    
[RequireComponent(typeof(Animator))]
public class ComboControllerBase : MonoBehaviour
{
    [Title("Combo")]
    [SerializeField] protected ComboList m_comboList;
    [SerializeField, ReadOnly] protected int m_currentComboIndex;
    [SerializeField, ReadOnly] protected int m_nextComboIndex;
    
    protected Animator m_animator;

    protected bool m_canEnableCombo = true;
    private CancellationTokenSource m_comboEnableCdCts;
    
    protected bool m_canExcuteCombo = true;
    private CancellationTokenSource m_comboExcuteCdCts;

    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    protected virtual void Update() { }

    protected virtual void OnEnable() {  }

    protected virtual void OnDisable()
    {
        ClearComboEnableCdDelay();
        ClearComboCdDelay();
    }

    #region 功能函数

    protected void TryExcuteCombo()
    {
        bool isFirstTime = m_currentComboIndex <= 0;
        // 招式进入，cd判断
        if (isFirstTime && !m_canEnableCombo) return;
        // combo进入，cd判断
        if (!m_canExcuteCombo) return;
        // 数据更新
        m_currentComboIndex = m_nextComboIndex;
        var config = m_comboList.TryGetComboConfig(m_currentComboIndex);
        if (config)
        {
            // 动画播放
            m_animator.CrossFadeInFixedTime(config.clipName, 0.155f, 0, 0);
            // combo计时
            EnterComboCdDelay(config.cdDuring);
        }
        // index 更新
        UpdateNextComboIndex();
        // 招式cd计时
        if (isFirstTime) EnterComboEnableCdDelay(m_comboList.cdDuring);   
    }

    protected void UpdateNextComboIndex()
    {
        m_nextComboIndex++;
        if (m_nextComboIndex >= m_comboList.comboListCount)
        {
            m_nextComboIndex = 0;
        }
    }

    #endregion

    #region 定时器

    private async UniTask EnterComboEnableCdDelay(int msDelay)
    {
        m_canEnableCombo = false;
        m_comboEnableCdCts = new CancellationTokenSource();
        await UniTask.Delay(msDelay, cancellationToken: m_comboEnableCdCts.Token);
        m_canEnableCombo = true;
    }

    private void ClearComboEnableCdDelay()
    {
        m_comboEnableCdCts?.Cancel();
        m_comboEnableCdCts?.Dispose();
        m_comboEnableCdCts = null;
    }

    private async UniTask EnterComboCdDelay(int msDelay)
    {
        ClearComboCdDelay();
        m_canExcuteCombo = false;
        m_comboExcuteCdCts = new CancellationTokenSource();
        await UniTask.Delay(msDelay, cancellationToken: m_comboExcuteCdCts.Token);
        m_canExcuteCombo = true;
    }

    private void ClearComboCdDelay()
    {
        m_comboExcuteCdCts?.Cancel();
        m_comboExcuteCdCts?.Dispose();
        m_comboExcuteCdCts = null;
    }

    #endregion
}
