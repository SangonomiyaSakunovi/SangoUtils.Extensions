using System;
using UnityEngine;

namespace SangoUtils.Behaviours_Unity.Timers
{
    internal class TimerAsyncOperation : CustomYieldInstruction
    {
        public bool isDone { get; set; } = false;

        private event Action<TimerAsyncOperation> _onCompleteCallBack;

        public event Action<TimerAsyncOperation> completed
        {
            add
            {
                if (isDone)
                {
                    value(this);
                }
                else
                {
                    _onCompleteCallBack += value;
                }
            }
            remove
            {
                _onCompleteCallBack -= value;
            }
        }

        public override bool keepWaiting => !isDone;

        public void Call()
        {
            _onCompleteCallBack?.Invoke(this);
        }

        public void Cancel()
        {
            isDone = true;
            _onCompleteCallBack = null;
        }
    }
}
