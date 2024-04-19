using SangoUtils.FixedNums;
using System;
using System.Collections.Generic;

namespace SangoUtils.Physics
{
    public class FixedCylinderCollider : FixedBaseCollider
    {
        public FixedInt Radius { get; private set; }

        public FixedCylinderCollider(FixedColliderConfig cfg)
        {
            Position = cfg.Position;
            Radius = cfg.CylinderRadius;
            Name = cfg.Name;
        }

        public void CalcCollidersInteraction(List<FixedBaseCollider> colliders, ref FixedVector3 velocity, ref FixedVector3 borderAdjust)
        {
            if (velocity == FixedVector3.Zero)
            {
                return;
            }
            List<FixedCollisionInfo> collisionInfoList = new List<FixedCollisionInfo>();
            FixedVector3 normal = FixedVector3.Zero;
            FixedVector3 adj = FixedVector3.Zero;
            for (int i = 0; i < colliders.Count; i++)
            {
                if (DetectContact(colliders[i], ref normal, ref adj))
                {
                    FixedCollisionInfo info = new FixedCollisionInfo
                    {
                        Collider = colliders[i],
                        Normal = normal,
                        BorderAdjust = adj
                    };
                    collisionInfoList.Add(info);
                }
            }

            if (collisionInfoList.Count == 1)
            {
                FixedCollisionInfo info = collisionInfoList[0];
                velocity = CorrectVelocity(velocity, info.Normal);
                borderAdjust = info.BorderAdjust;
            }
            else if (collisionInfoList.Count > 1)
            {
                FixedVector3 centerNormal = FixedVector3.Zero;
#pragma warning disable CS8600,CS8601
                FixedCollisionInfo info = null;
                FixedNumArgs borderNormalAngle = CalcMaxNormalAngle(collisionInfoList, velocity, ref centerNormal, ref info);
#pragma warning restore CS8600,CS8601
                FixedNumArgs angle = FixedVector3.Angle(-velocity, centerNormal);
                if (angle > borderNormalAngle)
                {
                    velocity = CorrectVelocity(velocity, info.Normal);
                    FixedVector3 adjSum = FixedVector3.Zero;
                    for (int i = 0; i < collisionInfoList.Count; i++)
                    {
                        adjSum += collisionInfoList[i].BorderAdjust;
                    }
                    borderAdjust = adjSum;
                }
                else
                {
                    velocity = FixedVector3.Zero;
                }
            }
            else
            {
                throw new Exception();
            }
        }

        private FixedNumArgs CalcMaxNormalAngle(List<FixedCollisionInfo> infoList, FixedVector3 velocity, ref FixedVector3 centerNormal, ref FixedCollisionInfo info)
        {
            for (int i = 0; i < infoList.Count; i++)
            {
                centerNormal += infoList[i].Normal;
            }
            centerNormal /= infoList.Count;

            FixedNumArgs normalAngle = FixedNumArgs.Zero;
            FixedNumArgs velocityAngle = FixedNumArgs.Zero;
            for (int i = 0; i < infoList.Count; i++)
            {
                FixedNumArgs tmpNorAngle = FixedVector3.Angle(centerNormal, infoList[i].Normal);
                if (normalAngle < tmpNorAngle)
                {
                    normalAngle = tmpNorAngle;
                }
                FixedNumArgs tmpVelAngle = FixedVector3.Angle(velocity, infoList[i].Normal);
                if (velocityAngle < tmpVelAngle)
                {
                    velocityAngle = tmpVelAngle;
                    info = infoList[i];
                }
            }
            return normalAngle;
        }

        private FixedVector3 CorrectVelocity(FixedVector3 velocity, FixedVector3 normal)
        {
            if (normal == FixedVector3.Zero)
            {
                return velocity;
            }
            if (FixedVector3.Angle(normal, velocity) > FixedNumArgs.HALFPI)
            {
                FixedInt prjLen = FixedVector3.Dot(velocity, normal);
                if (prjLen != 0)
                {
                    velocity -= prjLen * normal;
                }
            }
            return velocity;
        }

        public override bool DetectBoxContact(FixedBoxCollider col, ref FixedVector3 normal, ref FixedVector3 borderAdjust)
        {
            FixedVector3 disOffset = Position - col.Position;

            FixedInt dot_disX = FixedVector3.Dot(disOffset, col.Directions[0]);
            FixedInt dot_disZ = FixedVector3.Dot(disOffset, col.Directions[2]);

            FixedInt clamp_x = FixedNumCalc.Clamp(dot_disX, -col.Size.X, col.Size.X);
            FixedInt clamp_z = FixedNumCalc.Clamp(dot_disZ, -col.Size.Z, col.Size.Z);

            FixedVector3 s_x = clamp_x * col.Directions[0];
            FixedVector3 s_z = clamp_z * col.Directions[2];

            FixedVector3 point = col.Position;
            point += s_x;
            point += s_z;

            FixedVector3 po = Position - point;
            po.Y = 0;

            if (FixedVector3.Pow(po) > Radius * Radius)
            {
                return false;
            }
            else
            {
                normal = po.Normalized;
                FixedInt len = po.Magnitude;
                borderAdjust = normal * (Radius - len);
                return true;
            }
        }

        public override bool DetectSphereContact(FixedCylinderCollider col, ref FixedVector3 normal, ref FixedVector3 borderAdjust)
        {
            FixedVector3 disOffset = Position - col.Position;
            if (FixedVector3.Pow(disOffset) > (Radius + col.Radius) * (Radius + col.Radius))
            {
                return false;
            }
            else
            {
                normal = disOffset.Normalized;
                borderAdjust = normal * (Radius + col.Radius - disOffset.Magnitude);
                return true;
            }
        }
    }
}
