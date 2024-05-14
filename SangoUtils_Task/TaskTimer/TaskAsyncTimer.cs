using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace SangoUtils.Tasks
{
    public class TaskAsyncTimer : TaskBaseTimer
    {
        /// <summary>
        /// If true, you need call the HandleTask method in Main Thread to handle the task callback.
        /// If false, all the tasks will invoke in the task thread, and you don't need to call the HandleTask method.
        /// </summary>
        private bool _isSetHandled;
        private readonly ConcurrentDictionary<uint, AsyncTimerTask> _taskDict;
        private readonly ConcurrentQueue<AsyncTimerTaskPack>? _taskPackQueue;

        private int _taskMaxCount;

        public TaskAsyncTimer(bool isSetHandled = true, int taskConcurrencyLevel = -1, int taskMaxCount = 10000)
        {
            _isSetHandled = isSetHandled;
            if (isSetHandled)
            {
                _taskPackQueue = new ConcurrentQueue<AsyncTimerTaskPack>();
            }
            _taskDict = new ConcurrentDictionary<uint, AsyncTimerTask>(taskConcurrencyLevel, 10000);
            _taskMaxCount = taskMaxCount;
        }

        /// <summary>
        /// If repeatTaskCount is less than 0, the task will repeat forever.
        /// </summary>
        public override uint AddTask(uint delayedInvokeTaskTime, Action<uint> onTaskUpdated, Action<uint> onTaskCompleted, Action<uint> onTaskCanceled, int repeatTaskCount = 1)
        {
            uint taskID = GenerateTaskID();
            AsyncTimerTask task = new AsyncTimerTask(taskID, delayedInvokeTaskTime, repeatTaskCount, onTaskUpdated, onTaskCompleted, onTaskCanceled);
            RunTaskInPool(task);

            if (taskID == 0)
            {
                LogErrorFunc?.Invoke($"TaskAsyncTimer AddTask Error: Dict is Full.");
                return 0;
            }
            else if (_taskDict.TryAdd(taskID, task))
            {
                return taskID;
            }
            else
            {
                LogWarningFunc?.Invoke($"TaskAsyncTimer AddTask Warnning: [ {taskID} ] already Exist.");
                return 0;
            }
        }

        public override bool RemoveTask(uint taskID)
        {
            if (_taskDict.TryRemove(taskID, out AsyncTimerTask task))
            {
                task.CancellationTokenSource.Cancel();
                OnTaskCanceled(taskID, task.OnTaskCanceled);
                LogInfoFunc?.Invoke($"TaskAsyncTimer RemoveTask Succeed: [ {taskID} ].");
                return true;
            }
            else
            {
                LogErrorFunc?.Invoke($"TaskAsyncTimer RemoveTask Error: Try Remove [ {task.TaskID} ] in TaskDic Failed.");
                return false;
            }
        }

        public override bool ResetTaskTimer()
        {
            if (_taskPackQueue != null && !_taskPackQueue.IsEmpty)
            {
                LogWarningFunc?.Invoke("TaskAsyncTimer ResetTask Warnning: TaskCallBack Queue is Not Empty.");
            }
            _taskDict.Clear();
            _taskID = 0;
            return true;
        }

        public override void HandleTask()
        {
            while (_taskPackQueue != null && _taskPackQueue.Count > 0)
            {
                if (_taskPackQueue.TryDequeue(out AsyncTimerTaskPack pack))
                {
                    pack.OnTask!.Invoke(pack.TaskID);
                }
                else
                {
                    LogWarningFunc?.Invoke($"TaskAsyncTimer HandleTask Warnning: TaskPackQueue Dequeue Failed.");
                }
            }
        }

        private void RunTaskInPool(AsyncTimerTask task)
        {
            Task.Run(async () =>
            {
                if (task.RepeatTaskCount > 0)
                {
                    do
                    {
                        --task.RepeatTaskCount;
                        ++task.LoopIndex;
                        int delay = (int)(task.DelayedInvokeTaskTime + task.FixedDeltaTime);
                        if (delay > 0)
                        {
                            await Task.Delay(delay, task.CancellationToken);
                        }
                        TimeSpan ts = DateTime.UtcNow - task.StartTime;
                        task.FixedDeltaTime = (int)(task.DelayedInvokeTaskTime * task.LoopIndex - ts.TotalMilliseconds);
                        if (task.RepeatTaskCount == 0)
                        {
                            OnTaskUpdated(task.TaskID, task.OnTaskUpdated);
                            OnTaskCompleted(task.TaskID, task.OnTaskCompleted);
                        }
                        else
                        {
                            OnTaskUpdated(task.TaskID, task.OnTaskUpdated);
                        }
                    } while (task.RepeatTaskCount > 0);
                }
                else
                {
                    while (true)
                    {
                        ++task.LoopIndex;
                        int delay = (int)(task.DelayedInvokeTaskTime + task.FixedDeltaTime);
                        if (delay > 0)
                        {
                            await Task.Delay(delay, task.CancellationToken);
                        }
                        TimeSpan ts = DateTime.UtcNow - task.StartTime;
                        task.FixedDeltaTime = (int)(task.DelayedInvokeTaskTime * task.LoopIndex - ts.TotalMilliseconds);
                        OnTaskUpdated(task.TaskID, task.OnTaskUpdated);
                    }
                }
            });
        }

        protected override void OnTaskUpdated(uint taskID, Action<uint>? action)
        {
            if (_isSetHandled && action != null)
            {
                _taskPackQueue!.Enqueue(new AsyncTimerTaskPack(taskID, action));
            }
            else
            {
                action?.Invoke(taskID);
            }
        }

        protected override void OnTaskCompleted(uint taskID, Action<uint>? action)
        {
            if (_taskDict.TryRemove(taskID, out AsyncTimerTask task))
            {
                if (_isSetHandled && action != null)
                {
                    _taskPackQueue!.Enqueue(new AsyncTimerTaskPack(taskID, action));
                }
                else
                {
                    action?.Invoke(taskID);
                }
            }
            else
            {
                LogErrorFunc?.Invoke($"TaskAsyncTimer UpdateTask Error: Remove [ {taskID} ] in TaskDic Failed.");
            }
        }

        protected override void OnTaskCanceled(uint taskID, Action<uint>? action)
        {
            if (_isSetHandled && action != null)
            {
                _taskPackQueue!.Enqueue(new AsyncTimerTaskPack(taskID, action));
            }
            else
            {
                action?.Invoke(taskID);
            }
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

        private class AsyncTimerTask : BaseTimerTask
        {
            public DateTime StartTime { get; private set; }
            public ulong LoopIndex { get; set; }
            public int FixedDeltaTime { get; set; }
            public CancellationTokenSource CancellationTokenSource { get; private set; }
            public CancellationToken CancellationToken { get; private set; }

            public AsyncTimerTask(uint taskID, uint delayedInvokeTime, int repeatCount, Action<uint> onTaskUpdated, Action<uint> onTaskCompleted, Action<uint> onTaskCanceled) :
                base(taskID, delayedInvokeTime, repeatCount, onTaskUpdated, onTaskCompleted, onTaskCanceled)
            {
                StartTime = DateTime.UtcNow;
                LoopIndex = 0;
                FixedDeltaTime = 0;
                CancellationTokenSource = new CancellationTokenSource();
                CancellationToken = CancellationTokenSource.Token;
            }
        }

        private class AsyncTimerTaskPack : BaseTimerTaskPack
        {
            public AsyncTimerTaskPack(uint taskID, Action<uint> onTask) :
                base(taskID, onTask)
            {

            }
        }
    }
}