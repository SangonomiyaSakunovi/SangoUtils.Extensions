using UnityEngine;

namespace SangoUtils.Bases_Unity
{
    public abstract class BasePanel : MonoBehaviour
    {
        public void SetPanelState(bool isActive = true)
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

        public PanelLayer PanelLayer { get; protected set; }

        protected void AddPanel<T>(T panel) where T : BasePanel
        {
            UIService.Instance.AddPanel<T>(panel);
        }
    }
}
