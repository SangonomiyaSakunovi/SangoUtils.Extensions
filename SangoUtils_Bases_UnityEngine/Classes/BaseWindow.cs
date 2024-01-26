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

        protected abstract void OnInit();

        protected abstract void OnDispose();
    }
}
