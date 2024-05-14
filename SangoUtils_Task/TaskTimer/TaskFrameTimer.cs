using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SangoUtils.Tasks
{
    public class TaskFrameTimer : TaskBaseTimer
    {
        private ulong _currentFrame;
        private readonly Dictionary<uint, FrameTimerTask> _taskDict;
        private readonly List<uint> _taskRemoveLts = new List<uint>();

        private int _taskMaxCount;

        public TaskFrameTimer(ulong frameId = 0, int taskMaxCount = 10000)
        {
            _currentFrame = frameId;
            _taskDict = new Dictionary<uint, FrameTimerTask>(taskMaxCount);
            _taskMaxCount = taskMaxCount;
        }

        public override uint AddTask(uint delayedInvokeTaskTime, Action<uint> onTaskUpdated, Action<uint> onTaskCompleted, Action<uint> onTaskCanceled, int repeatTaskCount = 1)
        {
            uint taskID = GenerateTaskID();
            ulong destFrame = _currentFrame + delayedInvokeTaskTime;
            FrameTimerTask task = new FrameTimerTask(taskID, delayedInvokeTaskTime, repeatTaskCount, destFrame, onTaskUpdated, onTaskCompleted, onTaskCanceled);
            if (taskID == 0)
            {
                LogErrorFunc?.Invoke($"TaskFrameTimer AddTask Error: Dict is Full.");
                return 0;
            }
            else if (!_taskDict.ContainsKey(taskID))
            {
                _taskDict.Add(taskID, task);
                return taskID;
            }
            else
            {
                LogWarningFunc?.Invoke($"TaskFrameTimer AddTask Warnning: [ {taskID} ] already Exist.");
                return 0;
            }
        }

        public override bool RemoveTask(uint taskID)
        {
            if (_taskDict.TryGetValue(taskID, out FrameTimerTask task))
            {
                if (_taskDict.Remove(taskID))
                {
                    OnTaskCanceled(taskID, task.OnTaskCanceled);
                    LogInfoFunc?.Invoke($"TaskFrameTimer RemoveTask Succeed: [ {taskID} ].");
                    return true;
                }
                else
                {
                    LogErrorFunc?.Invoke($"TaskFrameTimer RemoveTask Error: Try Remove [ {taskID} ] in TaskDic Failed.");
                    return false;
                }
            }
            else
            {
                LogWarningFunc?.Invoke($"TaskFrameTimer RemoveTask Warnning: [ {taskID} ] is Not Exist.");
                return false;
            }
        }

        public override bool ResetTaskTimer()
        {
            _taskDict.Clear();
            _taskRemoveLts.Clear();
            _currentFrame = 0;
            return true;
        }

        public override void HandleTask()
        {
            ++_currentFrame;
            _taskRemoveLts.Clear();

            foreach (FrameTimerTask task in _taskDict.Values)
            {
                if (task.RepeatTaskCount > 0)
                {
                    if (task.TargetFrame <= _currentFrame)
                    {
                        OnTaskUpdated(task.TaskID, task.OnTaskUpdated);
                        task.TargetFrame += task.DelayedInvokeTaskTime;
                        --task.RepeatTaskCount;

                        if (task.RepeatTaskCount == 0)
                        {
                            OnTaskCompleted(task.TaskID, task.OnTaskCompleted);
                            _taskRemoveLts.Add(task.TaskID);
                        }
                    }
                }
                else
                {
                    OnTaskUpdated(task.TaskID, task.OnTaskUpdated);
                }
            }

            for (int i = 0; i < _taskRemoveLts.Count; i++)
            {
                if (_taskDict.Remove(_taskRemoveLts[i]))
                {
                    LogInfoFunc?.Invoke($"TaskFrameTimer UpdateTask Succeed: [ {_taskRemoveLts[i]} ] Run to Completion.");
                }
                else
                {
                    LogErrorFunc?.Invoke($"TaskFrameTimer UpdateTask Error: Remove [ {_taskRemoveLts[i]} ] in TaskDic Failed.");
                }
            }
        }

        protected override void OnTaskUpdated(uint taskID, Action<uint>? action)
        {
            action?.Invoke(taskID);
        }

        protected override void OnTaskCompleted(uint taskID, Action<uint>? action)
        {
            action?.Invoke(taskID);
        }

        protected override void OnTaskCanceled(uint taskID, Action<uint>? action)
        {
            action?.Invoke(taskID);
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

        private class FrameTimerTask : BaseTimerTask
        {
            public ulong TargetFrame;

            public FrameTimerTask(uint taskID, uint delayInvokeTime, int repeatCount, ulong targetFrame, Action<uint> onTaskUpdated, Action<uint> onTaskCompleted, Action<uint> onTaskCanceled) :
                base(taskID, delayInvokeTime, repeatCount, onTaskUpdated, onTaskCompleted, onTaskCanceled)
            {
                TargetFrame = targetFrame;
            }
        }
    }
}