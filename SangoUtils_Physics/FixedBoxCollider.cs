using SangoUtils_FixedNum;
using System;

namespace SangoUtils_Physics
{
    public class FixedBoxCollider : FixedBaseCollider
    {
        public FixedVector3 Size { get; private set; }
        public FixedVector3[] Directions;

        public FixedBoxCollider(FixedColliderConfig cfg)
        {
            Position = cfg.Position;
            Size = cfg.BoxSize;
            Directions = new FixedVector3[3];
            if (cfg.BoxAxis != null && cfg.BoxAxis.Length > 2)
            {
                Directions[0] = cfg.BoxAxis[0];
                Directions[1] = cfg.BoxAxis[1];
                Directions[2] = cfg.BoxAxis[2];
            }
            else
            {
                throw new Exception();
            }
            Name = cfg.Name;
        }
        public override bool DetectBoxContact(FixedBoxCollider col, ref FixedVector3 normal, ref FixedVector3 borderAdjust)
        {
            //分离轴算法TODO
            return false;
        }

        public override bool DetectSphereContact(FixedCylinderCollider col, ref FixedVector3 normal, ref FixedVector3 borderAdjust)
        {
            FixedVector3 tmpNormal = FixedVector3.Zero;
            FixedVector3 tmpAdjust = FixedVector3.Zero;
            bool result = col.DetectBoxContact(this, ref tmpNormal, ref tmpAdjust);
            normal = -tmpNormal;
            borderAdjust = -tmpAdjust;
            return result;
        }
    }
}
