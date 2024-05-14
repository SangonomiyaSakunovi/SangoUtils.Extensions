using SangoUtils.Bases_Unity.ComponentHelpers;
using System;
using UnityEngine;

namespace SangoUtils.Bases_Unity.Utils
{
    public static class ComponentsHelperUtils
    {
        public static void AddComponentsHelper<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            Component component = gameObject.AddComponent<T>();
            if (typeof(IComponentsHelper).IsAssignableFrom(typeof(T)))
            {
                IComponentsHelper? meta = component as IComponentsHelper;
                if (meta != null)
                {
                    meta.OnInitialize();
                }
            }
        }

        public static void AddComponentsHelper(this GameObject gameObject, Type type)
        {
            Component component = gameObject.AddComponent(type);
            if (typeof(IComponentsHelper).IsAssignableFrom(type))
            {
                IComponentsHelper? meta = component as IComponentsHelper;
                if (meta != null)
                {
                    meta.OnInitialize();
                }
            }
        }

        public static void RemoveComponentsHelper<T>(this GameObject gameObject) where T : MonoBehaviour
        {
            Component component = gameObject.GetComponent<T>();
            if (component != null)
            {
                if (typeof(IComponentsHelper).IsAssignableFrom(typeof(T)))
                {
                    IComponentsHelper? meta = component as IComponentsHelper;
                    if (meta != null)
                    {
                        Type[] types = meta.GetReleventComponents();
                        MonoBehaviour.DestroyImmediate(component);
                        if (types != null)
                        {
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
            }
        }

        public static void RemoveComponentsHelper(this GameObject gameObject, Type type)
        {
            Component component = gameObject.GetComponent(type);
            if (component != null)
            {
                if (typeof(IComponentsHelper).IsAssignableFrom(type))
                {
                    IComponentsHelper? meta = component as IComponentsHelper;
                    if (meta != null)
                    {
                        Type[] types = meta.GetReleventComponents();
                        MonoBehaviour.DestroyImmediate(component);
                        if (types != null)
                        {
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
    }
}
