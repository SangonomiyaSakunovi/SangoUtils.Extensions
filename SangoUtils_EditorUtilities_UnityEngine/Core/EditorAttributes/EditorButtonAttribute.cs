﻿#if UNITY_EDITOR
using System;

namespace SangoUtils.EditorWindows_Unity
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