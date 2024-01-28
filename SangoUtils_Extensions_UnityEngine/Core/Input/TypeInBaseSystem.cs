using SangoUtils_Extensions_UnityEngine.Service;
using UnityEngine;

namespace SangoUtils_Extensions_UnityEngine.Core
{
    public class TypeInBaseSystem : MonoBehaviour
    {
        public virtual void SetSystem()
        {
        }

        public virtual void ShowKeyboard() { }

        public virtual void HideKeyboard() { }

        public virtual void SetKeyboardDirection(KeyboradDirectionCode directionCode) { }
    }
}
