using SangoUtils.Extensions_Unity.Service;
using UnityEngine;

namespace SangoUtils.Extensions_Unity.Core
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
