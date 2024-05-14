using SangoUtils.Bases_Unity;
using SangoUtils.Tasks;
using System;
using UnityEngine;

namespace SangoUtils.Engines_Unity.Services
{
    public class TaskTimerService : BaseService<TaskTimerService>
    {
        private TaskAsyncTimer? _taskAsyncTimer;
        private TaskTickTimer? _taskTickTimer;
        private TaskFrameTimer? _taskFrameTimer;

        public void Initialize()
        {
            _taskAsyncTimer = new TaskAsyncTimer(true, 16);
            _taskAsyncTimer.LogInfoFunc = Debug.Log;
            _taskAsyncTimer.LogErrorFunc = Debug.LogError;
            _taskAsyncTimer.LogWarningFunc = Debug.LogWarning;

            //_taskTickTimer = new TaskTickTimer(true, 10);
            //_taskTickTimer.LogInfoFunc = Debug.Log;
            //_taskTickTimer.LogErrorFunc = Debug.LogError;
            //_taskTickTimer.LogWarningFunc = Debug.LogWarning;

            //_taskFrameTimer = new TaskFrameTimer();
            //_taskFrameTimer.LogInfoFunc = Debug.Log;
            //_taskFrameTimer.LogErrorFunc = Debug.LogError;
            //_taskFrameTimer.LogWarningFunc = Debug.LogWarning;
        }

        public uint AddTaskTimer<T>(uint delayedInvokeTaskTime, Action<uint> onTaskUpdated, Action<uint> onTaskCompleted, Action<uint> onTaskCanceled, int repeatTaskCount = 1) where T : TaskBaseTimer => typeof(T).Name switch
        {
            nameof(TaskAsyncTimer) => _taskAsyncTimer!.AddTask(delayedInvokeTaskTime, onTaskUpdated, onTaskCompleted, onTaskCanceled, repeatTaskCount),
            nameof(TaskTickTimer) => _taskTickTimer!.AddTask(delayedInvokeTaskTime, onTaskUpdated, onTaskCompleted, onTaskCanceled, repeatTaskCount),
            nameof(TaskFrameTimer) => _taskFrameTimer!.AddTask(delayedInvokeTaskTime, onTaskUpdated, onTaskCompleted, onTaskCanceled, repeatTaskCount),
            _ => 0
        };

        public bool RemoveTaskTimer<T>(uint taskID) where T : TaskBaseTimer => typeof(T).Name switch
        {
            nameof(TaskAsyncTimer) => _taskAsyncTimer!.RemoveTask(taskID),
            nameof(TaskTickTimer) => _taskTickTimer!.RemoveTask(taskID),
            nameof(TaskFrameTimer) => _taskFrameTimer!.RemoveTask(taskID),
            _ => false
        };

        public bool ResetTaskTimer<T>() where T : TaskBaseTimer => typeof(T).Name switch
        {
            nameof(TaskAsyncTimer) => _taskAsyncTimer!.ResetTaskTimer(),
            nameof(TaskTickTimer) => _taskTickTimer!.ResetTaskTimer(),
            nameof(TaskFrameTimer) => _taskFrameTimer!.ResetTaskTimer(),
            _ => false
        };

        private void Update()
        {
            _taskAsyncTimer?.HandleTask();
            //_taskTickTimer?.HandleTask();
        }

        public void UpdateByFrame()
        {
            //_taskFrameTimer?.HandleTask();
        }
    }
}
