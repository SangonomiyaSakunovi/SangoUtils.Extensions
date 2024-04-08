#if UNITY_EDITOR
using System.Reflection;
using UnityEngine;

namespace SangoUtils.Editors
{
    internal static class EditorWindowReflectionUtils
    {
        public static FieldInfo[]? GetAllFields(object target)
        {
            if (target == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts.");
                return null;
            }

            return target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy);
        }

        public static MethodInfo[]? GetAllMethods(object target)
        {
            if (target == null)
            {
                Debug.LogError("The target object is null. Check for missing scripts.");
                return null;
            }

            return target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy);
        }
    }
}
#endif