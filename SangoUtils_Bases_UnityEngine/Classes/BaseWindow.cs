using UnityEngine;

namespace SangoUtils_Bases_UnityEngine
{
    public abstract class BaseWindow : MonoBehaviour
    {
        public void SetWindowState(bool isActive = true)
        {
            if (gameObject.activeSelf != isActive)
            {
                gameObject.SetActive(isActive);
                if (isActive)
                {
                    OnInit();
                }
                else
                {
                    OnDispose();
                }
            }
        }

        protected abstract void OnAwake();

        protected abstract void OnInit();

        protected abstract void OnDispose();

        public WindowLayer WindowLayer { get; set; } = WindowLayer.None;

        private void Awake()
        {
            OnAwake();
        }

        protected void AddWindow<T>(T window) where T : BaseWindow
        {
            UIService.Instance.AddWindow<T>(window);
        }
    }
}
