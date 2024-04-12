#if UNITY_EDITOR
using System;

namespace SangoUtils.Editors
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class EditorButtonAttribute : Attribute
    {
        public string Text { get; private set; }

        public EditorButtonAttribute(string text = null)
        {
            this.Text = text;
        }
    }
}
#endif