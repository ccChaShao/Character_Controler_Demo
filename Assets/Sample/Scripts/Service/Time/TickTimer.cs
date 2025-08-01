using System;
using System.Collections.Concurrent;
using System.Threading;


class TimerTask
{
    public int tid;
    public int time;
    public Action taskCB;
    public int count;
    public double destTime;
    public double startTime;

    public long countIndex;
    public TimerTask(int tid, int time, Action taskCB, int count, double startTime, double destTime)
    {
        this.tid = tid;
        this.time = time;
        this.taskCB = taskCB;
        this.count = count;
        this.destTime = destTime;
        this.startTime = startTime;
        countIndex = 0;
    }
}

class TaskCBPack
{
    public int tid;
    public Action tack;
    public TaskCBPack(int tid, Action tack)
    {
        this.tid = tid;
        this.tack = tack;
    }
}

public class TickTimer : GameTimerBase
{
    private readonly ConcurrentDictionary<int, TimerTask> taskDic;
    private readonly ConcurrentQueue<TaskCBPack> taskCBPackQue;
    private readonly DateTime startDateTime = new DateTime(1970,1,1,1,0,0,0,0);
    private Thread thread;
    private readonly CancellationTokenSource tokenSource;

    public TickTimer(int interval = 0)
    {
        taskDic = new ConcurrentDictionary<int, TimerTask>();
        if (interval != 0)
        {
           tokenSource = new CancellationTokenSource();
            void StartTime()
            {
                try
                {
                    while (!tokenSource.IsCancellationRequested)
                    {
                        UpdateTime();
                        Thread.Sleep(interval);
                    }
                }
                catch(ThreadAbortException e )
                {
                    warnAction?.Invoke("TickTime StartTime Thread Abort Failed"+e);
                }
            }
            thread = new Thread(new ThreadStart(StartTime));
            thread.Start();
        }
    }

    public void UpdateTime()
    {
        double newTime = GetNewUTCMilliSecond();
        foreach (var task in taskDic)
        {
            TimerTask timerTask = task.Value;
            if (newTime < timerTask.destTime)
            {
                continue;
            }
            ++timerTask.countIndex;
            
            if (timerTask.count > 0)
            {
                --timerTask.count;
                if (timerTask.count == 0)
                {
                    FinishTimer(timerTask.tid);
                }
                else
                {
                    timerTask.destTime = timerTask.startTime + (timerTask.countIndex + 1) * timerTask.time;
                    timerTask.taskCB?.Invoke();
                }
            }
            // 小于0无限触发
            else
            {
                timerTask.destTime = timerTask.startTime + (timerTask.countIndex + 1) * timerTask.time;
                timerTask.taskCB?.Invoke();
            }
        }
    }

    private void FinishTimer(int tid)
    {
        if (taskDic.TryRemove(tid, out TimerTask timerTask))
        {
            timerTask.taskCB?.Invoke();
        }
        else
        {
            errorAction?.Invoke($"remove timerTask tid:{tid} failed");
        }
    }

    public override int AddTimer(int time, Action taskCB, int count = 1)
    {
        int tid = GenerateTid();
        double startTime = GetNewUTCMilliSecond();
        double destTime = startTime + time;
        TimerTask timerTask = new TimerTask(tid, time, taskCB, count, startTime, destTime);
        if (taskDic.TryAdd(tid, timerTask))
        {
            return tid;
        }
        return -1;
    }
    public override bool DeleteTimer(int tid)
    {
        return taskDic.TryRemove(tid, out _);
    }

    public override void ResetTimer()
    {
       taskDic.Clear();
        if (tokenSource != null)
        {
            tokenSource.Cancel();

            if (thread != null&&thread.IsAlive)
            {
                thread.Join();
            }
        }
      
    }
    private double GetNewUTCMilliSecond()
    {
        TimeSpan ts = DateTime.UtcNow - startDateTime;
        return ts.TotalMilliseconds;
    }
    
    protected override int GenerateTid()
    {
        lock (taskDic)
        {
            while (true)
            {
                ++tid;
                if (tid > int.MaxValue)
                {
                    tid = 0;
                }
                if (!taskDic.ContainsKey(tid))
                {
                    return tid;
                }
            }
        }
    }
}
