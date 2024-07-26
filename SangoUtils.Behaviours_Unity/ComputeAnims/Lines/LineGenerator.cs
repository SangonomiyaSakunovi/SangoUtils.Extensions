using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SangoUtils.Behaviours_Unity.ComputeAnims.Lines
{
    internal static class LineGenerator
    {
        /// <summary>
        /// 一阶贝塞尔曲线
        /// </summary>
        /// <param name="a">点坐标</param>
        /// <param name="b">点坐标</param>
        /// <param name="t">时间参数，范围0-1</param>
        /// <returns></returns>
        public static Vector3 LineBezier(Vector3 a, Vector3 b, float t)
        {
            return a + (b - a) * t;
        }

        /// <summary>
        /// 二阶贝塞尔曲线
        /// </summary>
        /// <param name="a">点坐标</param>
        /// <param name="b">点坐标</param>
        /// <param name="c">点坐标</param>
        /// <param name="t">时间参数，范围0-1</param>
        /// <returns></returns>
        public static Vector3 QuardaticBezier(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 aa = a + (b - a) * t;
            Vector3 bb = b + (c - b) * t;
            return aa + (bb - aa) * t;
        }

        /// <summary>
        /// 三阶贝塞尔曲线
        /// </summary>
        /// <param name="a">点坐标</param>
        /// <param name="b">点坐标</param>
        /// <param name="c">点坐标</param>
        /// <param name="d">点坐标</param>
        /// <param name="t">时间参数，范围0-1</param>
        /// <returns></returns>
        public static Vector3 CubicBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            Vector3 aa = a + (b - a) * t;
            Vector3 bb = b + (c - b) * t;
            Vector3 cc = c + (d - c) * t;

            Vector3 aaa = aa + (bb - aa) * t;
            Vector3 bbb = bb + (cc - bb) * t;
            return aaa + (bbb - aaa) * t;
        }
    }
}
