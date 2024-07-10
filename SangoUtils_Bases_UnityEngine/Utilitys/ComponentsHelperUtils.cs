using SangoUtils.Bases_Unity.ComponentsHelpers;
using System;
using UnityEngine;

namespace SangoUtils.Bases_Unity.Utils
{
    public static class ComponentsHelperUtils
    {
        public static void AddComponentsHelper<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            Component component = gameObject.AddComponent<T>();
            if (component is IComponentsHelper meta)
            {
                meta.OnInitialize();
            }
        }

        public static void AddComponentsHelper(this GameObject gameObject, Type type)
        {
            Component component = gameObject.AddComponent(type);
            if (component is IComponentsHelper meta)
            {
                meta.OnInitialize();
            }
        }

        public static void RemoveComponentsHelper<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            if (gameObject.TryGetComponent<T>(out var component))
            {
                if (component is IComponentsHelper meta)
                {
                    Type[] types = meta.GetReleventComponents();
                    MonoBehaviour.DestroyImmediate(component);
                    foreach (var type in types)
                    {
                        var componentNeighbor = gameObject.GetComponent(type);
                        if (componentNeighbor != null)
                        {
                            MonoBehaviour.DestroyImmediate(componentNeighbor);
                        }
                    }
                }
            }
        }

        public static void RemoveComponentsHelper(this GameObject gameObject, Type type)
        {
            Component component = gameObject.GetComponent(type);
            if (component != null)
            {
                if (component is IComponentsHelper meta)
                {
                    Type[] types = meta.GetReleventComponents();
                    MonoBehaviour.DestroyImmediate(component);
                    foreach (var childType in types)
                    {
                        var componentNeighbor = gameObject.GetComponent(childType);
                        if (componentNeighbor != null)
                        {
                            MonoBehaviour.DestroyImmediate(componentNeighbor);
                        }
                    }
                }
            }
        }
    }
}
