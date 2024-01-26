using UnityEngine;

namespace SangoUtils_Bases_UnityEngine
{
    public abstract class BaseService<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T? _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(T)) as T;
                    if (_instance == null)
                    {
                        GameObject gameObject = new GameObject("[" + typeof(T).FullName + "]");
                        _instance = gameObject.AddComponent<T>();
                        gameObject.hideFlags = HideFlags.HideInHierarchy;
                        DontDestroyOnLoad(gameObject);
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (null != _instance && _instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
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
