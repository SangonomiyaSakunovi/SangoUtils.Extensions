using System.Reflection;

namespace SangoUtils.Bases_Unity.Utils
{
    internal static class MonobehaviorReflectionUtils
    {
        public static FieldInfo[]? GetAllFields(object target)
        {
            if (target == null)
            {
                UnityEngine.Debug.LogError("The target object is null. Check for missing scripts.");
                return null;
            }

            return target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy);
        }

        public static MethodInfo[]? GetAllMethods(object target)
        {
            if (target == null)
            {
                UnityEngine.Debug.LogError("The target object is null. Check for missing scripts.");
                return null;
            }

            return target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy);
        }
    }
}
