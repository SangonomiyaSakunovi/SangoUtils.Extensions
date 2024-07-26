using System;
using System.Runtime.CompilerServices;

namespace SangoUtils.Behaviours_Unity.ComputeAnims.Lerps
{
    internal class LerpAnim
    {
        /// <summary>
        /// Lerp函数第三个参数，用于确定插值位置
        /// </summary>
        /// <param name="distance">插值两点间距离</param>
        /// <param name="speed">抽象移动速度</param>
        /// <param name="elapsedTime">插值累计时间</param>
        /// <returns>float t</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetSinDisplacementLerpIndex(float distance, float speed, float elapsedTime)
        {
            float fullTime = distance / speed;
            return (float)(0.5 + 0.5 * MathF.Sin((elapsedTime / fullTime * MathF.PI) - (MathF.PI / 2)));
        }

        /// <summary>
        /// Lerp函数第三个参数，用于确定插值位置
        /// </summary>
        /// <param name="distance">插值两点间距离</param>
        /// <param name="speed">抽象移动速度</param>
        /// <param name="elapsedTime">插值累计时间</param>
        /// <returns>float t</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetSinVelocityLerpIndex(float distance, float speed, float elapsedTime)
        {
            return (float)(0.5 - 0.5 * MathF.Cos(2 * speed / distance * elapsedTime));
        }
    }
}
