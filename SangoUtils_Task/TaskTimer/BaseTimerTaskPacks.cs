using System;

namespace SangoUtils.Tasks
{
    internal abstract class BaseTimerTask
    {
        public uint TaskID { get; private set; }
        public uint DelayedInvokeTaskTime { get; private set; }
        public int RepeatTaskCount { get; set; }
        public Action<uint>? OnTaskUpdated { get; private set; }
        public Action<uint>? OnTaskCompleted { get; private set; }
        public Action<uint>? OnTaskCanceled { get; private set; }

        public BaseTimerTask(uint taskID, uint delayedInvokeTaskTime, int repeatTaskCount, Action<uint> onTaskUpdated, Action<uint> onTaskCompleted, Action<uint> onTaskCanceled)
        {
            TaskID = taskID;
            DelayedInvokeTaskTime = delayedInvokeTaskTime;
            RepeatTaskCount = repeatTaskCount;
            OnTaskUpdated = onTaskUpdated;
            OnTaskCompleted = onTaskCompleted;
            OnTaskCanceled = onTaskCanceled;
        }
    }

    internal abstract class BaseTimerTaskPack
    {
        public uint TaskID { get; private set; }
        public Action<uint>? OnTask { get; private set; }

        protected BaseTimerTaskPack(uint taskID, Action<uint> onTask)
        {
            TaskID = taskID;
            OnTask = onTask;
        }
    }
}
