using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerService : MonoSingleton<TimerService>
{
    public TickTimer tickTimer { get; private set; }

    private void Awake()
    {
        tickTimer = new TickTimer();
    }

    private void Update()
    {
        tickTimer.UpdateTime();
    }

    private void OnDestroy()
    {
        tickTimer.ResetTimer();
    }
    
    /// <summary>
    /// 添加计时任务
    /// </summary>
    /// <param name="time">定时事件，单位毫秒</param>
    /// <param name="taskCB">定时CallBack任务</param>
    /// <param name="count">循环次数，小于等于0，代表无限次循环</param>
    /// <returns></returns>
    public int AddTimer(int time, Action taskCB, int count = 1)
    {
        return tickTimer.AddTimer(time, taskCB, count);
    }
    
    /// <summary>
    /// 移除计时任务，通过Tid参数注销
    /// </summary>
    /// <param name="tid"></param>
    public void RemoveTimer(int tid)
    {
        tickTimer.DeleteTimer(tid);
    }
}
