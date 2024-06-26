﻿#if UNITY_EDITOR
using System;

namespace SangoUtils.EditorWindows_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EditorObjectFieldAttribute : Attribute
    {
        public string Text { get; private set; }

        public EditorObjectFieldAttribute(string text = null)
        {
            this.Text = text;
        }
    }
}
#endif