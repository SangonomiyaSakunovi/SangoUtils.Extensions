﻿using UnityEngine;

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
    }
}
