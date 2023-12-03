﻿namespace SangoUtils_Server
{
    public class BaseService<T> : ServerSingleton<T> where T : class, new()
    {
        public override void Init()
        {
            base.Init();
            OnInit();
        }

        public override void Update()
        {
            base.Update();
            OnUpdate();
        }

        public virtual void OnInit()
        {

        }

        protected virtual void OnUpdate()
        {

        }

        public virtual void OnDispose()
        {

        }
    }
}
