using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace SangoUtils.Tasks
{
    public class TaskCompletedSequenceRunner : TaskBaseSequenceRunner
    {
        private readonly ConcurrentDictionary<uint, CompletedSequenceTask> _taskDict = new ConcurrentDictionary<uint, CompletedSequenceTask>();

        private uint _taskID = 1;
        private const string _taskIdLock = "TaskCompleteSequence_Lock";

        public uint AddTask(List<uint> prerequisitedTasks, Action<uint> doneTaskCallBack, Action<uint> cancelTaskCallBack, int repeatTaskCount = 1)
        {
            uint taskID = GenerateTaskId();
            //CompleteSequenceTask task = new CompleteSequenceTask();
            return 0;

        }

        public bool RemoveTask(uint taskID)
        {
            throw new NotImplementedException();
        }

        public void ResetTask()
        {
            throw new NotImplementedException();
        }

        protected uint GenerateTaskId()
        {
            lock (_taskIdLock)
            {
                while (true)
                {
                    ++_taskID;
                    if (_taskID == uint.MaxValue)
                    {
                        _taskID = 1;
                    }
                    if (!_taskDict.ContainsKey(_taskID))
                    {
                        return _taskID;
                    }
                }
            }
        }

        private class CompletedSequenceTask
        {
            public uint taskID;
            public List<uint> prerequisitedTasks;
            public Action<uint>? completeCallBack;
            public Action<uint>? cancelCallBack;

            public CancellationTokenSource cancellationTokenSource;
            public CancellationToken cancellationToken;

            public CompletedSequenceTask(uint taskId, List<uint> prerequisitedTasks)
            {
                this.taskID = taskId;
                this.prerequisitedTasks = prerequisitedTasks;
                cancellationTokenSource = new CancellationTokenSource();
                cancellationToken = cancellationTokenSource.Token;
            }
        }

        private class CompletedSequenceTaskPack
        {

        }
    }
}