using System;
using System.Collections.Generic;

namespace SangoUtils_Bases_UnityEngine
{
    public class UIService : BaseService<UIService>
    {
        public Action<string>? LogErrorFunc { get; set; }

        private readonly Dictionary<int, BaseWindow> _windowsDict = new Dictionary<int, BaseWindow>();

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
    }

    public enum WindowLayer
    {
        None,
        Base,
        Pop,
        Top
    }
}
