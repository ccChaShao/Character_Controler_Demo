using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DelayTaskService
{
    private Action m_delayAction;
    private CancellationTokenSource m_delayCts;
    private UniTask _task;
    
    public async UniTask RunDelay(int msDelay, Action onComplete)
    {
        m_delayAction = onComplete;
        m_delayCts?.Cancel();           // 中断前一个任务
        m_delayCts = new CancellationTokenSource();
        try
        {
            await UniTask.Delay(msDelay, cancellationToken: m_delayCts.Token);
            m_delayAction?.Invoke();
        }
        catch (OperationCanceledException) { }
    }
    
    public void Cancel() => m_delayCts?.Cancel();
}
