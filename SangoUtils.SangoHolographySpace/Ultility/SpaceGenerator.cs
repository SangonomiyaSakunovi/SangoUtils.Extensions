using UnityEngine;

namespace SangoUtils.HolographySpace_Unity
{
    public static class SpaceGenerator
    {
        /// <summary>
        /// Generate subspace from 4 points
        /// </summary>
        /// <param name="p0">Point0</param>
        /// <param name="p1">Point1</param>
        /// <param name="p2">Point2</param>
        /// <param name="p3">Point3</param>
        /// <param name="parentTrans">ParentTrans</param>
        /// <returns>The Subspace Object</returns>
        public static GameObject GenerateSubspace(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, Transform parentTrans)
        {
            Vector3 res = SpacePointUtils.CalcDiagonalIntersection(p0, p1, p2, p3);

            GameObject root = new GameObject("Subspace");
            root.transform.SetParent(parentTrans);
            root.transform.localPosition = res;

            return root;
        }
    }
}
