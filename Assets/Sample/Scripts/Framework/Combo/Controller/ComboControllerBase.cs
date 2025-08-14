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
    [SerializeField, LabelText("招式列表资源")] private SOComboList m_comboList;

    [SerializeField, LabelText("预输入开启")] private bool m_enablePreInput;
    
    protected Animator animator;
    protected int curComboIndex;
    protected bool enablePreInput => m_enablePreInput;

    // 招式进入
    protected bool canExcuteCombo = true;
    private CancellationTokenSource m_comboExcuteCdCts;
    
    // 招式推进
    protected bool canMoveNextCombo = true;
    private CancellationTokenSource m_comboMoveNextCdCts;

    // 招式间隔
    private CancellationTokenSource m_gapComboCdCts;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Update() { }

    protected virtual void OnEnable() {  }

    protected virtual void OnDisable()
    {
        ClearComboExcuteDelay();
        ClearMoveNextComboDelay();
        ClearComboGapDelay();
    }

    #region 功能函数

    protected void TryExcuteCombo()
    {
        bool isFirstTime = curComboIndex <= 0;
        
        // 招式进入，cd判断
        if (isFirstTime && !canExcuteCombo)
        {
            return;
        }
        // combo进入，cd判断
        if (!canMoveNextCombo)
        {
            return;
        }
        int comboIndex = (curComboIndex + 1) >= m_comboList.comboListCount ? 0 : curComboIndex + 1;
        var config = m_comboList.TryGetComboConfig(curComboIndex);
        if (config)
        {
            // 动画播放
            animator.CrossFadeInFixedTime(config.clipName, 0.155f, 0, 0);
            // combo计时
            EnterMoveNextComboDelay(config.cdDuring);
        }
        // 数据更新
        curComboIndex = comboIndex;         // 更新下标
        m_enablePreInput = false;           // 关闭预输入
        // 招式cd计时
        if (isFirstTime) EnterComboExcuteDelay((int)m_comboList.cdDuring);            // 截断小数点
        // 招式gap计时
        EnterComboGapDelay((int)m_comboList.gapDuring);           // 截断小数点
    }

    protected void ResetComboIndex()
    {
        curComboIndex = 0;
    }

    #endregion

    #region 定时器

    private async UniTask EnterComboGapDelay(int msDelay)
    {
        ClearComboGapDelay();
        m_gapComboCdCts = new();
        await UniTask.Delay(msDelay, cancellationToken: m_gapComboCdCts.Token);
        OnComboOverGap_Inside();
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
        canExcuteCombo = false;
        m_comboExcuteCdCts = new();
        await UniTask.Delay(msDelay, cancellationToken: m_comboExcuteCdCts.Token);
        canExcuteCombo = true;
    }

    private void ClearComboExcuteDelay()
    {
        m_comboExcuteCdCts?.Cancel();
        m_comboExcuteCdCts?.Dispose();
        m_comboExcuteCdCts = null;
    }

    private async UniTask EnterMoveNextComboDelay(int msDelay)
    {
        ClearMoveNextComboDelay();
        canMoveNextCombo = false;
        m_comboMoveNextCdCts = new();
        await UniTask.Delay(msDelay, cancellationToken: m_comboMoveNextCdCts.Token);
        canMoveNextCombo = true;
    }

    private void ClearMoveNextComboDelay()
    {
        m_comboMoveNextCdCts?.Cancel();
        m_comboMoveNextCdCts?.Dispose();
        m_comboMoveNextCdCts = null;
    }

    #endregion

    #region Inside Life

    private void OnComboOverGap_Inside()
    {
        // 超出间隙 - 招式顺序清空
        ResetComboIndex();
        // 传递生命周期
        OnComboOverGap();
    }

    #endregion

    #region Outside Life

    public virtual void OnComboOverGap() { }

    #endregion

    #region Anim Event

    public virtual void EnablePreInput()
    {
        m_enablePreInput = true;
    }

    #endregion
}
