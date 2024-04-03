using System;

namespace SangoUtils.Editors_Unity
{
    public enum GUIInfoBoxType
    {
        Normal,
        Warning,
        Error
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class GUIInfoBoxAttribute : SangoGUIDrawerAttribute
    {
        public string Text { get; private set; }
        public GUIInfoBoxType Type { get; private set; }

        public GUIInfoBoxAttribute(string text, GUIInfoBoxType type = GUIInfoBoxType.Normal)
        {
            Text = text;
            Type = type;
        }
    }
}
