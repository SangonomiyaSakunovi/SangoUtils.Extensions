using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SangoUtils.HolographySpace_Unity
{
    public static class SpacePointUtils
    {
        /// <summary>
        /// Calculate the intersection point of the diagonal of a quadrilateral
        /// </summary>
        /// <param name="p0">Vertex0 Position</param>
        /// <param name="p1">Vertex1 Position</param>
        /// <param name="p2">Vertex2 Position</param>
        /// <param name="p3">Vertex3 Position</param>
        /// <returns>The Center Position</returns>
        public static Vector3 CalcDiagonalIntersection(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 res = Vector3.zero;

            List<float> floatsX = new List<float> { p0.x, p1.x, p2.x, p3.x };
            List<float> floatsY = new List<float> { p0.y, p1.y, p2.y, p3.y };
            List<float> floatsZ = new List<float> { p0.z, p1.z, p2.z, p3.z };

            res.x = CalcAverage(floatsX);
            res.y = CalcAverage(floatsY);
            res.z = CalcAverage(floatsZ);

            return res;
        }

        public static float CalcAverage(List<float> floats)
        {
            return floats.Average();
        }
    }
}
