using UnityEngine;

namespace SangoUtils.Extensions_Unity.Core
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}