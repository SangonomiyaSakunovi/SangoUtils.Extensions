using System.Collections.Generic;

namespace SangoUtils.PhysicExtensions_Unity
{
    public class FixedEnvColliders
    {
        public List<FixedColliderConfig> EnvColliderConfigs { get; set; } = new List<FixedColliderConfig>();
        private readonly List<FixedBaseCollider> _envColliders = new List<FixedBaseCollider>();

        public void Init()
        {
            for (int i = 0; i < EnvColliderConfigs.Count; i++)
            {
                FixedColliderConfig cfg = EnvColliderConfigs[i];
                if (cfg.ColliderType == FixedColliderType.Box)
                {
                    _envColliders.Add(new FixedBoxCollider(cfg));
                }
                else if (cfg.ColliderType == FixedColliderType.Cylinder)
                {
                    _envColliders.Add(new FixedCylinderCollider(cfg));
                }
                else
                {
                    //TODO
                }
            }
        }

        public List<FixedBaseCollider> GetEnvColliders()
        {
            return _envColliders;
        }
    }
}
