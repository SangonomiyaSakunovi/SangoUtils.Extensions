﻿using UnityEngine;

namespace SangoUtils.Bases_Unity
{
    public abstract class BaseRoot<T> : MonoBehaviour
    {
        private void Update()
        {
            OnUpdate();
        }

        public abstract void OnAwake();

        public abstract void OnInit();

        protected abstract void OnUpdate();

        public abstract void OnDispose(); 
    }
}
