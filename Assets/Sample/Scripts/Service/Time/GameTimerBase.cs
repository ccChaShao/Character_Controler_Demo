using System;

public abstract class GameTimerBase 
{
    public Action<string> logAction;
    public Action<string> warnAction;
    public Action<string> errorAction;
    
    protected int tid;
    public abstract int AddTimer(int time, Action taskCB, int count = 1);
    public abstract bool DeleteTimer(int tid);
    public abstract void ResetTimer();
    protected abstract int GenerateTid();
}