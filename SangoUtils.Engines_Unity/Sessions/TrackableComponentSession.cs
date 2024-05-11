using SangoUtils.Bases_Unity.Trackables;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SangoUtils.Engines_Unity
{
    public class TrackableComponentSession : MonoBehaviour
    {
#pragma warning disable CS8618
        public static TrackableComponentSession Instance { get; private set; }
#pragma warning restore CS8618
        public Action<string> LogErrorFunc { get; set; } = Debug.LogError;

        private readonly Dictionary<int, TrackabeWindowPack> _windowsDict = new Dictionary<int, TrackabeWindowPack>();
        private readonly Dictionary<int, TrackablePanelPack> _panelsDict = new Dictionary<int, TrackablePanelPack>();

        /// <summary>
        /// Warning: You must call this API before using any other APIs.
        /// </summary>
        public void Initialize()
        {
            if (Instance != null)
            {
                LogErrorFunc("TrackableComponentSession is already initialized.");
                return;
            }

            Instance = this;
        }

        public void AddComponent()
        {
            Scene scene = SceneManager.GetActiveScene();
            GameObject[] rootObjects = scene.GetRootGameObjects();
            AddComponent(rootObjects);
        }

        public void AddComponent(GameObject[] rootObjects)
        {
            for (int i = 0; i < rootObjects.Length; i++)
            {
                AddComponent(rootObjects[i]);
            }
        }

        public void AddComponent(GameObject rootObject)
        {
            Component[] components = rootObject.GetComponentsInChildren<Component>(true);
            AddComponentAsTrackables(components);
        }

        public void AddComponentAsTrackables(Component[] components)
        {
            foreach (var component in components)
            {
                if (component != null)
                {
                    Type type = component.GetType();
                    AddComponentAsTrackableWindow(component, type);
                    AddComponentAsTrackablePanel(component, type);
                }
            }
        }

        public void AddComponentAsTrackableWindow(Component component, Type type)
        {
            if (Attribute.IsDefined(type, typeof(TrackableWindowAttribute)) && typeof(ITrackableWindow).IsAssignableFrom(type))
            {
                GameObject trackableObject = component.gameObject;
                int trackableID = type.GetHashCode();
                TrackableWindowAttribute attribute = (TrackableWindowAttribute)Attribute.GetCustomAttribute(type, typeof(TrackableWindowAttribute));
                int trackableGroupID = attribute.TrackableGroupID;
                
                if (!_windowsDict.ContainsKey(trackableID))
                {
                    TrackabeWindowPack pack = new TrackabeWindowPack(trackableID, trackableGroupID, trackableObject);
                    ITrackableWindow? meta = component as ITrackableWindow;
                    if (meta != null)
                    {
                        pack.OnTrackableAwake = meta.OnTrackableAwake;
                        pack.OnTrackbaleEnable = meta.OnTrackbaleEnable;
                        pack.OnTrackableDisable = meta.OnTrackableDisable;
                        pack.OnTrackableDestroy = meta.OnTrackableDestroy;
                        pack.OnTrackableMessage = meta.OnTrackableMessage;
                    }
                    _windowsDict.Add(trackableID, pack);
                }
            }
        }

        public void AddComponentAsTrackablePanel(Component component, Type type)
        {
            if (Attribute.IsDefined(type, typeof(TrackablePanelAttribute)) && typeof(ITrackablePanel).IsAssignableFrom(type))
            {
                GameObject trackableObject = component.gameObject;
                int trackableID = type.GetHashCode();
                TrackablePanelAttribute attribute = (TrackablePanelAttribute)Attribute.GetCustomAttribute(type, typeof(TrackablePanelAttribute));
                int trackableGroupID = attribute.TrackableGroupID;
                
                if (!_panelsDict.ContainsKey(trackableID))
                {
                    TrackablePanelPack pack = new TrackablePanelPack(trackableID, trackableGroupID, trackableObject);
                    ITrackablePanel? meta = component as ITrackablePanel;
                    if (meta != null)
                    {
                        pack.OnTrackableAwake = meta.OnTrackableAwake;
                        pack.OnTrackbaleEnable = meta.OnTrackbaleEnable;
                        pack.OnTrackableDisable = meta.OnTrackableDisable;
                        pack.OnTrackableDestroy = meta.OnTrackableDestroy;
                        pack.OnTrackableMessage = meta.OnTrackableMessage;
                    }
                    _panelsDict.Add(trackableID, pack);
                }
            }
        }

        public void OpenWindow<T>(bool isCloseOther = true, params object[] messages) where T : ITrackableWindow
        {
            Type type = typeof(T);
            int windowId = type.GetHashCode();
            OpenWindow(windowId, isCloseOther, messages);
        }

        public void OpenPanel<T>(bool isCloseOther = true, params object[] messages) where T : ITrackablePanel
        {
            Type type = typeof(T);
            int panelId = type.GetHashCode();
            OpenPanel(panelId, isCloseOther, messages);
        }

        public void OpenWindow(int windowID, bool isCloseOther = true, params object[] messages)
        {
            if (isCloseOther)
            {
                CloseWindowAll();
            }

            if (_windowsDict.TryGetValue(windowID, out TrackabeWindowPack trackabeWindow))
            {
                if (trackabeWindow.TrackableObject?.activeSelf != true)
                {
                    if (messages != null && messages.Length > 0)
                    {
                        trackabeWindow.OnTrackableMessage?.Invoke(messages);
                    }

                    trackabeWindow.OnTrackbaleEnable?.Invoke();
                    trackabeWindow.TrackableObject?.SetActive(true);
                }
            }
        }

        public void OpenPanel(int panelID, bool isCloseOther = true, params object[] messages)
        {
            if (isCloseOther)
            {
                ClosePanelAll();
            }

            if (_panelsDict.TryGetValue(panelID, out TrackablePanelPack trackablePanel))
            {
                if (trackablePanel.TrackableObject?.activeSelf != true)
                {
                    if (messages != null && messages.Length > 0)
                    {
                        trackablePanel.OnTrackableMessage?.Invoke(messages);
                    }

                    trackablePanel.OnTrackbaleEnable?.Invoke();
                    trackablePanel.TrackableObject?.SetActive(true);
                }
            }
        }

        public void CloseWindow<T>(params object[] messages) where T : ITrackableWindow
        {
            Type windowType = typeof(T);
            int windowId = windowType.GetHashCode();
            CloseWindow(windowId, messages);
        }

        public void ClosePanel<T>(params object[] messages) where T : ITrackablePanel
        {
            Type panelType = typeof(T);
            int panelId = panelType.GetHashCode();
            ClosePanel(panelId, messages);
        }

        public void CloseWindow(int windowID, params object[] messages)
        {
            if (_windowsDict.TryGetValue(windowID, out TrackabeWindowPack trackabeWindow))
            {
                if (trackabeWindow.TrackableObject?.activeSelf != false)
                {
                    if (messages != null && messages.Length > 0)
                    {
                        trackabeWindow.OnTrackableMessage?.Invoke(messages);
                    }

                    trackabeWindow.TrackableObject?.SetActive(false);
                    trackabeWindow.OnTrackableDisable?.Invoke();
                }
            }
        }

        public void ClosePanel(int panelID, params object[] messages)
        {
            if (_panelsDict.TryGetValue(panelID, out TrackablePanelPack trackablePanel))
            {
                if (trackablePanel.TrackableObject?.activeSelf != false)
                {
                    if (messages != null && messages.Length > 0)
                    {
                        trackablePanel.OnTrackableMessage?.Invoke(messages);
                    }

                    trackablePanel.TrackableObject?.SetActive(false);
                    trackablePanel.OnTrackableDisable?.Invoke();
                }
            }
        }

        public void CloseWindowAll()
        {
            foreach (var pair in _windowsDict)
            {
                if (pair.Value.TrackableObject?.activeSelf != false)
                {
                    pair.Value.TrackableObject?.SetActive(false);
                    pair.Value.OnTrackableDisable?.Invoke();
                }
            }
        }

        public void ClosePanelAll()
        {
            foreach (var pair in _panelsDict)
            {
                if (pair.Value.TrackableObject?.activeSelf != false)
                {
                    pair.Value.TrackableObject?.SetActive(false);
                    pair.Value.OnTrackableDisable?.Invoke();
                }
            }
        }

        public void CloseWindowGroup(int windowGroupID)
        {
            foreach (var pair in _windowsDict)
            {
                if (pair.Value.TrackableGroupID == windowGroupID)
                {
                    if (pair.Value.TrackableObject?.activeSelf != false)
                    {
                        pair.Value.TrackableObject?.SetActive(false);
                        pair.Value.OnTrackableDisable?.Invoke();
                    }
                }
            }
        }

        public void ClosePanelGroup(int panelGroupID)
        {
            foreach (var pair in _panelsDict)
            {
                if (pair.Value.TrackableGroupID == panelGroupID)
                {
                    if (pair.Value.TrackableObject?.activeSelf != false)
                    {
                        pair.Value.TrackableObject?.SetActive(false);
                        pair.Value.OnTrackableDisable?.Invoke();
                    }
                }
            }
        }
    }
}
