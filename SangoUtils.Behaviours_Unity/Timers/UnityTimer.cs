using SangoUtils.Behaviours_Unity.Coroutines;
using System.Collections;
using UnityEngine;

namespace SangoUtils.Behaviours_Unity.Timers
{
    internal static class UnityTimer
    {
        public static TimerAsyncOperation WaitForSecondsRealtime(float duration)
        {
            TimerAsyncOperation operation = new TimerAsyncOperation();
            WaitYield(operation).Start();
            return operation;

            IEnumerator WaitYield(TimerAsyncOperation operation)
            {
                yield return new WaitForSecondsRealtime(duration);

                if (!operation.isDone)
                {
                    operation.isDone = true;
                    operation.Call();
                }
            }
        }

        public static TimerAsyncOperation RepeatWaitForSecondsRealtime(float duration)
        {
            TimerAsyncOperation operation = new TimerAsyncOperation();
            RepeatWaitYield(operation).Start();
            return operation;

            IEnumerator RepeatWaitYield(TimerAsyncOperation operation)
            {
                while (true)
                {
                    yield return new WaitForSecondsRealtime(duration);

                    if (!operation.isDone)
                    {
                        operation.Call();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
