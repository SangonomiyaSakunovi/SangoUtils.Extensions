using SangoUtils.FixedNums;

namespace SangoUtils.Physics
{
    public class FixedColliderConfig
    {
        public string Name { get; set; } = "";
        public FixedColliderType ColliderType { get; set; } = FixedColliderType.Default;
        public FixedVector3 Position { get; set; } = new FixedVector3(0, 0, 0);

        public FixedVector3 BoxSize { get; set; } = new FixedVector3(0, 0, 0);
        public FixedVector3[]? BoxAxis { get; set; }

        public FixedInt CylinderRadius { get; set; } = 0;
    }

    public enum FixedColliderType
    {
        Default,
        Box,
        Cylinder,
    }
}
