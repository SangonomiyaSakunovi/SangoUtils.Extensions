using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SangoUtils.Bases_Unity
{
    public class TrackableComponentSession : MonoBehaviour
    {
#pragma warning disable CS8618
        public static TrackableComponentSession Instance { get; private set; }
#pragma warning restore CS8618
        public Action<string> LogErrorFunc { get; set; } = Debug.LogError;

        private readonly Dictionary<int, TrackabeWindow> _windowsDict = new Dictionary<int, TrackabeWindow>();
        private readonly Dictionary<int, TrackablePanel> _panelsDict = new Dictionary<int, TrackablePanel>();

        public void InitSession()
        {
            Instance = this;
        }

        public void AddTrackableComponent()
        {
            Scene scene = SceneManager.GetActiveScene();
            GameObject[] rootObjects = scene.GetRootGameObjects();
            AddTrackableComponent(rootObjects);
        }

        public void AddTrackableComponent(GameObject[] rootObjects)
        {
            for (int i = 0; i < rootObjects.Length; i++)
            {
                AddTrackableComponent(rootObjects[i]);
            }
        }

        public void AddTrackableComponent(GameObject rootObject)
        {
            Component[] components = rootObject.GetComponentsInChildren<Component>(true);
            AddTrackableComponent(components);
        }

        public void AddTrackableComponent(Component[] components)
        {
            foreach (var component in components)
            {
                Type type = component.GetType();
                if (Attribute.IsDefined(type, typeof(TrackableWindowAttribute)) && typeof(ITrackableWindow).IsAssignableFrom(type))
                {
                    int trackableID = type.GetHashCode();
                    TrackableWindowAttribute attribute = (TrackableWindowAttribute)Attribute.GetCustomAttribute(type, typeof(TrackableWindowAttribute));
                    int trackableGroupID = attribute.TrackableGroupID;
                    GameObject trackableObject = component.gameObject;
                    if (!_windowsDict.ContainsKey(trackableID))
                    {
                        TrackabeWindow trackabeWindow = new TrackabeWindow(trackableID, trackableGroupID, trackableObject);
                        ITrackableWindow? trackableInterface = component as ITrackableWindow;
                        if (trackableInterface != null)
                        {
                            trackabeWindow.OnTrackableAwake = trackableInterface.OnTrackableAwake;
                            trackabeWindow.OnTrackbaleEnable = trackableInterface.OnTrackbaleEnable;
                            trackabeWindow.OnTrackableDisable = trackableInterface.OnTrackableDisable;
                            trackabeWindow.OnTrackableDestroy = trackableInterface.OnTrackableDestroy;
                            trackabeWindow.OnTrackableMessage = trackableInterface.OnTrackableMessage;
                        }
                        _windowsDict.Add(trackableID, trackabeWindow);
                    }
                }
                else if (Attribute.IsDefined(type, typeof(TrackablePanelAttribute)) && typeof(ITrackablePanel).IsAssignableFrom(type))
                {
                    int trackableID = type.GetHashCode();
                    TrackablePanelAttribute attribute = (TrackablePanelAttribute)Attribute.GetCustomAttribute(type, typeof(TrackablePanelAttribute));
                    int trackableGroupID = attribute.TrackableGroupID;
                    GameObject trackableObject = component.gameObject;
                    if (!_panelsDict.ContainsKey(trackableID))
                    {
                        TrackablePanel trackablePanel = new TrackablePanel(trackableID, trackableGroupID, trackableObject);
                        ITrackablePanel? trackableInterface = component as ITrackablePanel;
                        if (trackableInterface != null)
                        {
                            trackablePanel.OnTrackableAwake = trackableInterface.OnTrackableAwake;
                            trackablePanel.OnTrackbaleEnable = trackableInterface.OnTrackbaleEnable;
                            trackablePanel.OnTrackableDisable = trackableInterface.OnTrackableDisable;
                            trackablePanel.OnTrackableDestroy = trackableInterface.OnTrackableDestroy;
                            trackablePanel.OnTrackableMessage = trackableInterface.OnTrackableMessage;
                        }
                        _panelsDict.Add(trackableID, trackablePanel);
                    }
                }
            }
        }

        public void OpenWindow<T>(bool isCloseOther = true, params object[] messages) where T : ITrackableWindow
        {
            Type windowType = typeof(T);
            int windowId = windowType.GetHashCode();
            OpenWindow(windowId, isCloseOther, messages);
        }

        public void OpenPanel<T>(bool isCloseOther = true, params object[] messages) where T : ITrackablePanel
        {
            Type panelType = typeof(T);
            int panelId = panelType.GetHashCode();
            OpenPanel(panelId, isCloseOther, messages);
        }

        public void OpenWindow(int windowId, bool isCloseOther = true, params object[] messages)
        {
            if (isCloseOther)
            {
                CloseWindowAll();
            }

            if (_windowsDict.TryGetValue(windowId, out TrackabeWindow trackabeWindow))
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

        public void OpenPanel(int panelId, bool isCloseOther = true, params object[] messages)
        {
            if (isCloseOther)
            {
                ClosePanelAll();
            }

            if (_panelsDict.TryGetValue(panelId, out TrackablePanel trackablePanel))
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

        public void CloseWindow(int windowId, params object[] messages)
        {
            if (_windowsDict.TryGetValue(windowId, out TrackabeWindow trackabeWindow))
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

        public void ClosePanel(int panelId, params object[] messages)
        {
            if (_panelsDict.TryGetValue(panelId, out TrackablePanel trackablePanel))
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
