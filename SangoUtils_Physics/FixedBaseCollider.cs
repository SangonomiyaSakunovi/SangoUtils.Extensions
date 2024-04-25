using SangoUtils.FixedNums;

namespace SangoUtils.PhysicExtensions_Unity
{
    public abstract class FixedBaseCollider
    {
        public string Name { get; set; } = "";
        public FixedVector3 Position { get; set; } = new FixedVector3(0, 0, 0);

        public virtual bool DetectContact(FixedBaseCollider collider, ref FixedVector3 normal, ref FixedVector3 borderAdjust)
        {
            if (collider is FixedBoxCollider)
            {
                return DetectBoxContact((FixedBoxCollider)collider, ref normal, ref borderAdjust);
            }
            else if (collider is FixedCylinderCollider)
            {
                return DetectSphereContact((FixedCylinderCollider)collider, ref normal, ref borderAdjust);
            }
            else
            {
                //TODO:
                return false;
            }
        }
        public abstract bool DetectSphereContact(FixedCylinderCollider col, ref FixedVector3 normal, ref FixedVector3 borderAdjust);

        public abstract bool DetectBoxContact(FixedBoxCollider col, ref FixedVector3 normal, ref FixedVector3 borderAdjust);
    }

    public class FixedCollisionInfo
    {
        public FixedBaseCollider? Collider { get; set; }
        public FixedVector3 Normal { get; set; } = new FixedVector3(0, 0, 0);
        public FixedVector3 BorderAdjust { get; set; } = new FixedVector3(0, 0, 0);
    }
}
