using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SangoUtils.Behaviours_Unity.Pose
{
    internal static class PoseUtils
    {
        public static void SetPose(this Transform transform, in Pose pose, Space space = Space.World)
        {
            if (space == Space.World)
            {
                transform.SetPositionAndRotation(pose.position, pose.rotation);
                return;
            }

            transform.localRotation = pose.rotation;
            transform.localPosition = pose.position;
        }

        public static Pose GetPose(this Transform transform, Space space = Space.World)
        {
            if (space == Space.World)
            {
                return new Pose(transform.position, transform.rotation);
            }

            return new Pose(transform.localPosition, transform.localRotation);
        }

        public static void Multiply(in Pose a, in Pose b, ref Pose result)
        {
            result.position = a.position + a.rotation * b.position;
            result.rotation = a.rotation * b.rotation;
        }

        public static Pose Multiply(in Pose a, in Pose b)
        {
            Pose result = default(Pose);
            Multiply(in a, in b, ref result);
            return result;
        }

        public static void Premultiply(this ref Pose a, in Pose b)
        {
            Multiply(in a, in b, ref a);
        }

        public static void Postmultiply(this ref Pose a, in Pose b)
        {
            Multiply(in b, in a, ref a);
        }

        public static void Lerp(this ref Pose from, in Pose to, float t)
        {
            Lerp(in from, in to, t, ref from);
        }

        public static void Lerp(in Pose from, in Pose to, float t, ref Pose result)
        {
            result.position = Vector3.LerpUnclamped(from.position, to.position, t);
            result.rotation = Quaternion.SlerpUnclamped(from.rotation, to.rotation, t);
        }

        public static void Inverse(in Pose a, ref Pose result)
        {
            result.rotation = Quaternion.Inverse(a.rotation);
            result.position = result.rotation * -a.position;
        }

        public static void Invert(this ref Pose a)
        {
            Inverse(in a, ref a);
        }

        public static void CopyFrom(this ref Pose to, in Pose from)
        {
            to.position = from.position;
            to.rotation = from.rotation;
        }

        public static Pose Delta(this Transform from, Transform to)
        {
            return Delta(from.position, from.rotation, to.position, to.rotation);
        }

        public static Pose Delta(this Transform from, in Pose to)
        {
            return Delta(from.position, from.rotation, to.position, to.rotation);
        }

        public static void Delta(this Transform from, in Pose to, ref Pose result)
        {
            Delta(from.position, from.rotation, to.position, to.rotation, ref result);
        }

        public static Pose Delta(in Pose from, in Pose to)
        {
            return Delta(from.position, from.rotation, to.position, to.rotation);
        }

        private static Pose Delta(Vector3 fromPosition, Quaternion fromRotation, Vector3 toPosition, Quaternion toRotation)
        {
            Pose result = default(Pose);
            Delta(fromPosition, fromRotation, toPosition, toRotation, ref result);
            return result;
        }

        private static void Delta(Vector3 fromPosition, Quaternion fromRotation, Vector3 toPosition, Quaternion toRotation, ref Pose result)
        {
            Quaternion quaternion = Quaternion.Inverse(fromRotation);
            result.position = quaternion * (toPosition - fromPosition);
            result.rotation = quaternion * toRotation;
        }

        public static Pose GlobalPose(this Transform reference, in Pose offset)
        {
            return new Pose(reference.position + reference.rotation * offset.position, reference.rotation * offset.rotation);
        }

        public static Pose MirrorPoseRotation(this in Pose pose, Vector3 normal, Vector3 tangent)
        {
            Pose result = pose;
            Vector3 vector = pose.rotation * -Vector3.forward;
            Vector3 from = Vector3.ProjectOnPlane(vector, normal);
            float num = Vector3.SignedAngle(from, tangent, normal);
            Vector3 forward = Quaternion.AngleAxis(2f * num, normal) * vector;
            Vector3 vector2 = pose.rotation * -Vector3.up;
            Vector3 from2 = Vector3.ProjectOnPlane(vector2, normal);
            float num2 = Vector3.SignedAngle(from2, tangent, normal);
            Vector3 upwards = Quaternion.AngleAxis(2f * num2, normal) * vector2;
            result.rotation = Quaternion.LookRotation(forward, upwards);
            return result;
        }

        public struct Pose
        {
            public Vector3 position;

            public Quaternion rotation;

            public Pose(Vector3 position, Quaternion rotation)
            {
                this.position = position;
                this.rotation = rotation;
            }

            //
            // Summary:
            //     Transforms the current pose into the local space of the provided pose.
            //
            // Parameters:
            //   lhs:
            public Pose GetTransformedBy(Pose lhs)
            {
                Pose result = default(Pose);
                result.position = lhs.position + lhs.rotation * position;
                result.rotation = lhs.rotation * rotation;
                return result;
            }
        }
    }
}
