namespace SangoUtils.Bases
{
    public abstract class BaseSystem
    {
        public void Update()
        {
            OnUpdate();
        }

        public abstract void OnAwake();

        public abstract void OnInit();

        protected abstract void OnUpdate();

        public abstract void OnDispose();
    }
}
