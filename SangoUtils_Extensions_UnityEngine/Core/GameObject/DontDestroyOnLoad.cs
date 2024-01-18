using UnityEngine;

namespace SangoUtils_Extensions_UnityEngine.Core
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}