using System;

namespace SangoUtils.Tasks
{
    public abstract class TaskBaseTimer
    {
        public Action<string>? LogInfoFunc { get; set; }
        public Action<string>? LogWarningFunc { get; set; }
        public Action<string>? LogErrorFunc { get; set; }

        protected uint _taskID = 1;

        protected abstract uint GenerateTaskID();

        public abstract uint AddTask(uint delayedInvokeTaskTime, Action<uint> onTaskUpdated, Action<uint> onTaskCompleted, Action<uint> onTaskCanceled, int repeatTaskCount = 1);

        public abstract bool RemoveTask(uint taskID);

        public abstract void HandleTask();

        public abstract bool ResetTaskTimer();

        protected abstract void OnTaskUpdated(uint taskID, Action<uint>? action);

        protected abstract void OnTaskCompleted(uint taskID, Action<uint>? action);

        protected abstract void OnTaskCanceled(uint taskID, Action<uint>? action);
    }
}