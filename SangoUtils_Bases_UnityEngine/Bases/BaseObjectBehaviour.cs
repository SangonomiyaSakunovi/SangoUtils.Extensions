using SangoUtils.Bases_Unity.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace SangoUtils.Bases_Unity
{
    public abstract class BaseObjectBehaviour : MonoBehaviour
    {
        protected Transform? MoveTargetPosition { get; set; } = null;

        protected virtual void Update()
        {
            UpdatePosition();
            UpdateRotation();
            UpdateScale();
        }

        private void UpdatePosition()
        {

        }

        private void UpdateRotation()
        {

        }

        private void UpdateScale()
        {

        }

        public List<Component> GetRelevantComponent()
        {
            return ComponentReflectionUtils.GetRelevantComponent(this);
        }
    }
}
