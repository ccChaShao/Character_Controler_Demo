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
    [SerializeField] private ComboList m_comboList;
    [SerializeField, ReadOnly] private int m_currentComboIndex;
    [SerializeField, ReadOnly] private int m_nextComboIndex;
    
    private Animator m_animator;
    private bool m_canExcuteCombo = true;
    private CancellationTokenSource m_comboCdDelayCts;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        // 清理异步
        m_comboCdDelayCts?.Cancel();
        m_comboCdDelayCts?.Dispose();
        m_comboCdDelayCts = null;
    }

    private void ExcuteCombo()
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
        m_canExcuteCombo = false;
        // cd 更新
        EnterComboCdDelay(currentConfig.cdDuring);
    }

    private async UniTaskVoid EnterComboCdDelay(int msDelay)
    {
        m_canExcuteCombo = false;
        await UniTask.Delay(msDelay);
        m_canExcuteCombo = true;
    }

    private void UpdateNextComboIndex()
    {
        m_nextComboIndex++;
        if (m_nextComboIndex >= m_comboList.GetComboCount())
        {
            m_nextComboIndex = 0;
        }
    }
}
