using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

namespace SangoUtils_Extensions_UnityEngine.Core
{
    public static class GameObjectExtensions
    {
        public static void SetName(this GameObject gameObject, string name)
        {
            gameObject.name = name;
        }

        public static void LogGameObject(this GameObject root)
        {
            string debugString = root.GetFullName();

            root.ForEachChild((GameObject child) =>
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

        #region UIPointerListener
        public static void AddPointerClickListener(this GameObject gameObject, Action<PointerEventData, GameObject?, object[]?> onPointerClickCallBack, params object[] commands)
        {
            UIPointerListener listener = gameObject.GetComponent<UIPointerListener>() ?? gameObject.AddComponent<UIPointerListener>();
            listener.OnPointerClickCallBack1 = onPointerClickCallBack;
            listener.ListenerObject = gameObject;
            if (commands != null)
            {
                listener.Commands = commands;
            }
        }

        public static void AddPointerClickListener(this GameObject gameObject, Action<GameObject?, object[]?> onPointerClickCallBack, params object[] commands)
        {
            UIPointerListener listener = gameObject.GetComponent<UIPointerListener>() ?? gameObject.AddComponent<UIPointerListener>();
            listener.OnPointerClickCallBack0 = onPointerClickCallBack;
            listener.ListenerObject = gameObject;
            if (commands != null)
            {
                listener.Commands = commands;
            }
        }

        public static void AddPointerDownListener(this GameObject gameObject, Action<PointerEventData, GameObject?, object[]?> onPointerDownCallBack, params object[] commands)
        {
            UIPointerListener listener = gameObject.GetComponent<UIPointerListener>() ?? gameObject.AddComponent<UIPointerListener>();
            listener.OnPointerDownCallBack1 = onPointerDownCallBack;
            listener.ListenerObject = gameObject;
            if (commands != null)
            {
                listener.Commands = commands;
            }
        }

        public static void AddPointerDownListener(this GameObject gameObject, Action<GameObject?, object[]?> onPointerDownCallBack, params object[] commands)
        {
            UIPointerListener listener = gameObject.GetComponent<UIPointerListener>() ?? gameObject.AddComponent<UIPointerListener>();
            listener.OnPointerDownCallBack0 = onPointerDownCallBack;
            listener.ListenerObject = gameObject;
            if (commands != null)
            {
                listener.Commands = commands;
            }
        }

        public static void AddPointerUpListener(this GameObject gameObject, Action<PointerEventData, GameObject?, object[]?> onPointerUpCallBack, params object[] commands)
        {
            UIPointerListener listener = gameObject.GetComponent<UIPointerListener>() ?? gameObject.AddComponent<UIPointerListener>();
            listener.OnPointerUpCallBack1 = onPointerUpCallBack;
            listener.ListenerObject = gameObject;
            if (commands != null)
            {
                listener.Commands = commands;
            }
        }

        public static void AddPointerUpListener(this GameObject gameObject, Action<GameObject?, object[]?> onPointerUpCallBack, params object[] commands)
        {
            UIPointerListener listener = gameObject.GetComponent<UIPointerListener>() ?? gameObject.AddComponent<UIPointerListener>();
            listener.OnPointerUpCallBack0 = onPointerUpCallBack;
            listener.ListenerObject = gameObject;
            if (commands != null)
            {
                listener.Commands = commands;
            }
        }

        public static void AddPointerDragListener(this GameObject gameObject, Action<PointerEventData, GameObject?, object[]?> onPointerDragCallBack, params object[] commands)
        {
            UIPointerListener listener = gameObject.GetComponent<UIPointerListener>() ?? gameObject.AddComponent<UIPointerListener>();
            listener.OnPointerDragCallBack1 = onPointerDragCallBack;
            listener.ListenerObject = gameObject;
            if (commands != null)
            {
                listener.Commands = commands;
            }
        }

        public static void AddPointerDragListener(this GameObject gameObject, Action<GameObject?, object[]?> onPointerDragCallBack, params object[] commands)
        {
            UIPointerListener listener = gameObject.GetComponent<UIPointerListener>() ?? gameObject.AddComponent<UIPointerListener>();
            listener.OnPointerDragCallBack0 = onPointerDragCallBack;
            listener.ListenerObject = gameObject;
            if (commands != null)
            {
                listener.Commands = commands;
            }
        }

        public static void AddPointerSlideListener(this GameObject gameObject, Action<GameObject?, object[]?> onPointerSlideCallBack, Action<GameObject?, object[]?> onPointerClickDoneCallBack, params object[] commands)
        {

            gameObject.AddPointerDownListener((PointerEventData eventData, GameObject? gameObjectPointerDown, object[]? strs) =>
            {
                if (gameObjectPointerDown != null)
                {
                    UIPointerListener listener = gameObjectPointerDown.GetComponent<UIPointerListener>() ?? gameObjectPointerDown.AddComponent<UIPointerListener>();
                    listener.ClickDownPosition = eventData.position;
                }
            });
            gameObject.AddPointerDragListener((PointerEventData eventData, GameObject? gameObjectPointerDrag, object[]? strs) =>
            {
                if (gameObjectPointerDrag != null)
                {
                    UIPointerListener listener = gameObjectPointerDrag.GetComponent<UIPointerListener>() ?? gameObjectPointerDrag.AddComponent<UIPointerListener>();
                    Vector2 direction = eventData.position - listener.ClickDownPosition;
                    onPointerSlideCallBack?.Invoke(gameObject, new object[] { direction });
                }
            },
            commands);
            gameObject.AddPointerUpListener((PointerEventData eventData, GameObject? gameObjectPointerUp, object[]? strs) =>
            {
                if (gameObjectPointerUp != null)
                {
                    UIPointerListener listener = gameObjectPointerUp.GetComponent<UIPointerListener>() ?? gameObjectPointerUp.AddComponent<UIPointerListener>();
                    Vector2 direction = eventData.position - listener.ClickDownPosition;
                    onPointerClickDoneCallBack?.Invoke(gameObject, new object[] { direction });
                }
            });
        }
        #endregion

        #region EventTrigger
        public static void SetGameObjectClickListener(this GameObject gameObject, UnityAction<BaseEventData> actionCallBack)
        {
            EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();
            for (int i = 0; i < eventTrigger.triggers.Count; i++)
            {
                if (eventTrigger.triggers[i].eventID == EventTriggerType.PointerClick)
                {
                    return;
                }
            }
            Entry onClick = new Entry()
            {
                eventID = EventTriggerType.PointerClick
            };
            onClick.callback.AddListener(actionCallBack);
            eventTrigger.triggers.Add(onClick);
        }

        public static void SetGameObjectDragBeginListener(this GameObject gameObject, UnityAction<BaseEventData> actionCallBack)
        {
            EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();
            for (int i = 0; i < eventTrigger.triggers.Count; i++)
            {
                if (eventTrigger.triggers[i].eventID == EventTriggerType.BeginDrag)
                {
                    return;
                }
            }
            Entry onDragBegin = new Entry()
            {
                eventID = EventTriggerType.BeginDrag
            };
            onDragBegin.callback.AddListener(actionCallBack);
            eventTrigger.triggers.Add(onDragBegin);
        }

        public static void SetGameObjectDragEndListener(this GameObject gameObject, UnityAction<BaseEventData> actionCallBack)
        {
            EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();
            for (int i = 0; i < eventTrigger.triggers.Count; i++)
            {
                if (eventTrigger.triggers[i].eventID == EventTriggerType.EndDrag)
                {
                    return;
                }
            }
            Entry onDragEnd = new Entry()
            {
                eventID = EventTriggerType.EndDrag
            };
            onDragEnd.callback.AddListener(actionCallBack);
            eventTrigger.triggers.Add(onDragEnd);
        }
        #endregion
    }
}