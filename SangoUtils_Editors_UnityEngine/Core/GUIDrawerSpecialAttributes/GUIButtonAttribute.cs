using System;

namespace SangoUtils.CustomEditors_Unity
{
    public enum GUIButtonEnableMode
    {
        Always,
        Editor,
        Playmode
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class GUIButtonAttribute : SangoGUIDrawerSpecialAttribute
    {
        public string Text { get; private set; }
        public GUIButtonEnableMode SelectedEnableMode { get; private set; }

        public GUIButtonAttribute(string text = null, GUIButtonEnableMode enabledMode = GUIButtonEnableMode.Always)
        {
            this.Text = text;
            this.SelectedEnableMode = enabledMode;
        }
    }
}
