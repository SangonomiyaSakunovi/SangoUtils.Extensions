using UnityEngine;
using UnityEngine.Events;

namespace SangoUtils.Behaviours_Unity.GameObjectOPs
{
    public static class GameObjectExtension
    {
        public static void LogGameObject(this GameObject root)
        {
            string debugString = root.GetFullName();

            root.ForEachChild((child) =>
            {
                Transform t = child.transform;

                debugString += $"\n\t{{{child.GetFullName()}}}";

                Component[] components = t.GetComponents<Component>();
                foreach (Component c in components)
                {
                    debugString += $"\n\t\t[{c.GetType()}]";
                }
            }, true, true);

            Debug.Log($"GO = {debugString}");
        }

        public static void ForEachChild(this GameObject root, UnityAction<GameObject> callback, bool includeRoot = false, bool recursive = true)
        {
            if (includeRoot)
            {
                callback.Invoke(root);
            }

            Transform transform = root.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                if (recursive)
                {
                    ForEachChild(child.gameObject, callback, true);
                }
            }
        }

        public static string GetFullName(this GameObject root)
        {
            string fullName = "";

            Transform current = root.transform;

            while (current != null)
            {
                if (fullName == "")
                {
                    fullName = current.name;
                }
                else
                {
                    fullName = $"{current.name}/{fullName}";
                }
                current = current.parent;
            }

            return fullName;
        }

        public static void DestroyChildren(this GameObject root)
        {
            Transform transform = root.transform;

            while (transform.childCount > 0)
            {
                Transform child = transform.GetChild(0);
                child.SetParent(null);
                GameObject.Destroy(child.gameObject);
            }
        }

        public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
        {
            T? t = gameObject.GetComponent<T>();
            if (t != null)
            {
                UnityEngine.Object.Destroy(t);
            }
        }
    }
}