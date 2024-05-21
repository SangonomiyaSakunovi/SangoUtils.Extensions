#if UNITY_EDITOR
using System;

namespace SangoUtils.CustomEditors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EditorTextFieldAttribute : Attribute
    {
        public string Text { get; private set; }

        public EditorTextFieldAttribute(string text = null)
        {
            this.Text = text;
        }
    }
}
#endif
