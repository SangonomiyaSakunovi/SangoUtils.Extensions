using UnityEngine;

namespace SangoUtils.Engines_Unity.Utilities
{
    public static class ObjectUtils_Unity
    {
        public static GameObject GetGameObject(UnityEngine.Object obj)
        {
            if (obj as GameObject != null)
            {
                return (GameObject)obj;
            }
            else if (obj as Component != null)
            {
                return ((Component)obj).gameObject;
            }
            else
            {
                return null;
            }
        }
    }
}
