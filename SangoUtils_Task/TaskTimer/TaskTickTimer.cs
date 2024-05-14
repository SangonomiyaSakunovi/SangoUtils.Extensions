using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SangoUtils.Tasks
{
    public class TaskTickTimer : TaskBaseTimer
    {
        private readonly ConcurrentDictionary<uint, TickTimerTask> _taskDict;
        /// <summary>
        /// If true, you need call the HandleTask method in Main Thread to handle the task callback.
        /// If false, all the tasks will invoke in the task thread, and you don't need to call the HandleTask method.
        /// </summary>
        private readonly bool _isSetHandled;
        private readonly DateTime _utcInitialDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private readonly ConcurrentQueue<TickTimerTaskPack>? _taskPackQueue;
        private readonly Thread? _taskTickTimerThread;

        private int _taskMaxCount;

        public TaskTickTimer(bool isSetHandled = true, int updateLoopTime = 10, int taskConcurrencyLevel = -1, int taskMaxCount = 10000)
        {
            _isSetHandled = isSetHandled;           
            if (isSetHandled)
            {
                _taskPackQueue = new ConcurrentQueue<TickTimerTaskPack>();
            }
            _taskDict = new ConcurrentDictionary<uint, TickTimerTask>(taskConcurrencyLevel, taskMaxCount);
            _taskMaxCount = taskMaxCount;

            void StartTickTimerTaskInThread()
            {
                try
                {
                    while (true)
                    {
                        UpdateTask();
                        Thread.Sleep(updateLoopTime);
                    }
                }
                catch (ThreadAbortException e)
                {
                    LogWarningFunc?.Invoke($"TaskTickTimer Thread Warinning: Thread Abort Error, Reason: {e}.");
                }
            }
            _taskTickTimerThread = new Thread(new ThreadStart(StartTickTimerTaskInThread));
            _taskTickTimerThread.Start();
        }

        public override uint AddTask(uint intervalTime, Action<uint> onTaskUpdated, Action<uint> onTaskCompleted, Action<uint> onTaskCanceled, int repeatTaskCount = 1)
        {
            uint taskID = GenerateTaskID();
            double startTime = GetUTCMilliseconds();
            double targetTime = startTime + intervalTime;
            TickTimerTask tickTimerTask = new TickTimerTask(taskID, intervalTime, repeatTaskCount, startTime, targetTime, onTaskUpdated, onTaskCompleted, onTaskCanceled);

            if (taskID == 0)
            {
                LogErrorFunc?.Invoke($"TaskTickTimer AddTask Error: Dict is Full.");
                return 0;
            }
            else if (_taskDict.TryAdd(taskID, tickTimerTask))
            {
                return taskID;
            }
            else
            {
                LogWarningFunc?.Invoke($"TaskTickTimer AddTask Warnning: [ {taskID} ] already Exist.");
                return 0;
            }
        }

        public override bool RemoveTask(uint taskID)
        {
            if (_taskDict.TryRemove(taskID, out TickTimerTask task))
            {
                OnTaskCanceled(taskID, task.OnTaskCanceled);
                LogInfoFunc?.Invoke($"TaskTickTimer RemoveTask Succeed: [ {taskID} ].");
                return true;
            }
            else
            {
                LogWarningFunc?.Invoke($"TaskTickTimer RemoveTask Warnning: Remove [ {taskID} ] Failed.");
                return false;
            }
        }

        public override bool ResetTaskTimer()
        {
            if (!_taskPackQueue!.IsEmpty)
            {
                LogWarningFunc?.Invoke("TaskTickTimer ResetTask Warnning: TaskCallback Queue is Not Empty.");
            }
            _taskDict.Clear();
            _taskTickTimerThread?.Abort();
            return true;
        }

        public override void HandleTask()
        {
            while (_taskPackQueue != null && _taskPackQueue.Count > 0)
            {
                if (_taskPackQueue.TryDequeue(out TickTimerTaskPack pack))
                {
                    pack.OnTask!.Invoke(pack.TaskID);
                }
                else
                {
                    LogWarningFunc?.Invoke("TaskTickTimer HandleTask Warnning: TickTaskPack Queue Dequeue Failed.");
                }
            }
        }

        private void UpdateTask()
        {
            double nowTime = GetUTCMilliseconds();
            foreach (TickTimerTask task in _taskDict.Values)
            {
                if (nowTime < task.TargetTime)
                {
                    continue;
                }

                ++task.LoopIndex;
                if (task.RepeatTaskCount > 0)
                {
                    --task.RepeatTaskCount;
                    if (task.RepeatTaskCount == 0)
                    {
                        OnTaskUpdated(task.TaskID, task.OnTaskUpdated);
                        OnTaskCompleted(task.TaskID, task.OnTaskCompleted);
                    }
                    else
                    {
                        task.TargetTime = task.StartTime + task.DelayedInvokeTaskTime * (task.LoopIndex + 1);
                        OnTaskUpdated(task.TaskID, task.OnTaskUpdated);
                    }
                }
                else
                {
                    task.TargetTime = task.StartTime + task.DelayedInvokeTaskTime * (task.LoopIndex + 1);
                    OnTaskUpdated(task.TaskID, task.OnTaskUpdated);
                }
            }
        }

        protected override void OnTaskUpdated(uint taskID, Action<uint>? action)
        {
            if (_isSetHandled && action != null)
            {
                _taskPackQueue!.Enqueue(new TickTimerTaskPack(taskID, action));
            }
            else
            {
                action?.Invoke(taskID);
            }
        }

        protected override void OnTaskCompleted(uint taskID, Action<uint>? action)
        {
            if (_taskDict.TryRemove(taskID, out TickTimerTask task))
            {

                if (_isSetHandled && action != null)
                {
                    _taskPackQueue!.Enqueue(new TickTimerTaskPack(taskID, action));
                }
                else
                {
                    action?.Invoke(taskID);
                }
            }
            else
            {
                LogErrorFunc?.Invoke($"TaskTickTimer Done Error: Remove [ {taskID} ] in TaskDict Failed.");
            }
        }

        protected override void OnTaskCanceled(uint taskID, Action<uint>? action)
        {
            if (_isSetHandled && action != null)
            {
                _taskPackQueue!.Enqueue(new TickTimerTaskPack(taskID, action));
            }
            else
            {
                action?.Invoke(taskID);
            }
        }

        private double GetUTCMilliseconds()
        {
            TimeSpan timeSpan = DateTime.UtcNow - _utcInitialDateTime;
            return timeSpan.TotalMilliseconds;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected override uint GenerateTaskID()
        {
            int counter = 0;
            while (true)
            {
                ++_taskID;
                ++counter;
                if (_taskID == uint.MaxValue)
                {
                    _taskID = 1;
                }
                if (counter < _taskMaxCount)
                {
                    if (!_taskDict.ContainsKey(_taskID))
                    {
                        return _taskID;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        private class TickTimerTask : BaseTimerTask
        {
            public double TargetTime;
            public double StartTime;
            public ulong LoopIndex;

            public TickTimerTask(uint taskID, uint delayInvokeTime, int repeatCount, double startTime, double targetTime, Action<uint> onTaskUpdated, Action<uint> onTaskCompleted, Action<uint> onTaskCanceled) :
                base(taskID, delayInvokeTime, repeatCount, onTaskUpdated, onTaskCompleted, onTaskCanceled)
            {
                TargetTime = targetTime;
                StartTime = startTime;
                LoopIndex = 0;
            }
        }

        private class TickTimerTaskPack : BaseTimerTaskPack
        {
            public TickTimerTaskPack(uint taskID, Action<uint> onTask) :
                base(taskID, onTask)
            {

            }
        }
    }
}