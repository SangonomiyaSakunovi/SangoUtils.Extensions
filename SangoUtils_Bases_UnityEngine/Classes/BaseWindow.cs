using UnityEngine;

namespace SangoUtils.Bases_Unity
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

        public abstract void OnAwake();

        protected abstract void OnInit();

        protected abstract void OnDispose();

        public WindowLayer WindowLayer { get; protected set; }
    }
}
