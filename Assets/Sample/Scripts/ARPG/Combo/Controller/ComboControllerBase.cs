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
    [BoxGroup("Combo Box Broup")]
    [SerializeField] protected ComboList m_comboList;
    [SerializeField, ReadOnly] protected int m_currentComboIndex;
    [SerializeField, ReadOnly] protected int m_nextComboIndex;
    
    protected Animator m_animator;
    protected bool m_canExcuteCombo = true;
    protected CancellationTokenSource m_comboCdDelayCts;

    protected virtual void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    protected virtual void Update() { }

    protected virtual void OnEnable() {  }

    protected virtual void OnDisable()
    {
        // 清理异步
        m_comboCdDelayCts?.Cancel();
        m_comboCdDelayCts?.Dispose();
        m_comboCdDelayCts = null;
    }

    protected void ExcuteCombo()
    {
        // 数据更新
        m_currentComboIndex = m_nextComboIndex;
        var currentConfig = m_comboList.TryGetComboConfig(m_currentComboIndex);
        if (!currentConfig)
        {
            return;
        }
        // 动画播放
        m_animator.CrossFadeInFixedTime(currentConfig.clipName, 0.155f, 0, 0);
        // index 更新
        UpdateNextComboIndex();
        // cd 更新
        EnterComboCdDelay(currentConfig.cdDuring);
    }

    private async UniTaskVoid EnterComboCdDelay(int msDelay)
    {
        m_canExcuteCombo = false;
        await UniTask.Delay(msDelay);
        m_canExcuteCombo = true;
    }

    protected void UpdateNextComboIndex()
    {
        m_nextComboIndex++;
        if (m_nextComboIndex >= m_comboList.GetComboCount())
        {
            m_nextComboIndex = 0;
        }
    }
}
