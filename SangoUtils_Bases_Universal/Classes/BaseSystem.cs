namespace SangoUtils_Bases_Universal
{
    public abstract class BaseSystem<T> where T : class
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
