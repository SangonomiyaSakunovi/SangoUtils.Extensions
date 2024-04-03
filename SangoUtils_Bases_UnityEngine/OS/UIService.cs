using System;
using System.Collections.Generic;
using UnityEngine;

namespace SangoUtils.Bases_Unity
{
    public class UIService : BaseService<UIService>
    {
        public Action<string>? LogErrorFunc { get; set; }

        private readonly Dictionary<int, BaseWindow> _windowsDict = new Dictionary<int, BaseWindow>();
        private readonly Dictionary<int, BasePanel> _panelsDict = new Dictionary<int, BasePanel>();

        public override void OnInit()
        {

        }

        protected override void OnUpdate()
        {

        }

        public override void OnDispose()
        {

        }

        public void SwitchWindow<T>() where T : BaseWindow
        {
            Type windowType = typeof(T);
            int windowId = windowType.GetHashCode();
            SwitchWindow(windowId);
        }

        public void SwitchWindow(int windowId)
        {            
            foreach(var pair in _windowsDict)
            {
                
                if (pair.Value.WindowLayer == WindowLayer.Base)
                {
                    if (pair.Key == windowId)
                    {
                        pair.Value.SetWindowState(true);
                    }
                    else
                    {
                        pair.Value.SetWindowState(false);
                    }
                }
            }
        }

        public void SwitchPanel<T>() where T : BasePanel
        {
            Type panelType = typeof(T);
            int panelId = panelType.GetHashCode();
            SwitchPanel(panelId);
        }

        public void SwitchPanel(int panelId)
        {
            foreach (var pair in _panelsDict)
            {
                if (pair.Value.PanelLayer == PanelLayer.Base)
                {
                    if (pair.Key == panelId)
                    {                        
                        pair.Value.SetPanelState(true);
                    }
                    else
                    {
                        pair.Value.SetPanelState(false);
                    }
                }
            }
        }

        public void AddWindow<T>(T window) where T : BaseWindow
        {
            Type windowType = typeof(T);
            int windowId = windowType.GetHashCode();
            AddWindow(windowId, window);
        }

        public void AddWindow(int windowId, BaseWindow window)
        {
            if (_windowsDict.ContainsKey(windowId))
            {
                LogErrorFunc?.Invoke($"Window {windowId} already exists!");
                return;
            }
            _windowsDict.Add(windowId, window);
        }

        public void AddWindows(GameObject windowRoot)
        {
            BaseWindow[] windows = windowRoot.GetComponentsInChildren<BaseWindow>(true);
            foreach (var window in windows)
            {
                if (Attribute.IsDefined(window.GetType(), typeof(TrackableWindowAttribute)))
                {
                    Type windowType = window.GetType();
                    int windowId = windowType.GetHashCode();
                    if (!_windowsDict.ContainsKey(windowId))
                    {
                        _windowsDict.Add(windowId, window);
                    }
                }
            }
        }

        public BaseWindow? GetWindow<T>() where T : BaseWindow
        {
            Type windowType = typeof(T);
            int windowId = windowType.GetHashCode();
            return GetWindow(windowId);
        }

        public BaseWindow? GetWindow(int windowId)
        {
            if (_windowsDict.TryGetValue(windowId, out BaseWindow window))
            {
                return window;
            }
            return null;
        }

        public void AddPanel<T>(T panel) where T : BasePanel
        {
            Type panelType = typeof(T);
            int panelId = panelType.GetHashCode();
            AddPanel(panelId, panel);
        }

        public void AddPanel(int panelId, BasePanel panel)
        {
            if (_panelsDict.ContainsKey(panelId))
            {
                LogErrorFunc?.Invoke($"Panel {panelId} already exists!");
                return;
            }
            _panelsDict.Add(panelId, panel);
        }

        public void AddPanels(GameObject panelRoot)
        {
            BasePanel[] panels = panelRoot.GetComponentsInChildren<BasePanel>(true);
            foreach (var panel in panels)
            {
                if (Attribute.IsDefined(panel.GetType(), typeof(TrackablePanelAttribute)))
                {
                    Type panelType = panel.GetType();
                    int panelId = panelType.GetHashCode();
                    if (!_panelsDict.ContainsKey(panelId))
                    {
                        _panelsDict.Add(panelId, panel);
                    }
                }
            }
        }

        public BasePanel? GetPanel<T>() where T : BasePanel
        {
            Type panelType = typeof(T);
            int panelId = panelType.GetHashCode();
            return GetPanel(panelId);
        }

        public BasePanel? GetPanel(int panelId)
        {
            if (_panelsDict.TryGetValue(panelId, out BasePanel panel))
            {
                return panel;
            }
            return null;
        }
    }

    public enum WindowLayer
    {
        None,
        Base,
        Pop,
        Top
    }

    public enum PanelLayer
    {
        None,
        Base,
        Pop,
        Top
    }

    [AttributeUsage(AttributeTargets.Class)] 
    public class TrackableWindowAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public class TrackablePanelAttribute : Attribute { }
}
