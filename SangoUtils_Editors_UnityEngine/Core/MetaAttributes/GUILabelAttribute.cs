﻿using System;

namespace SangoUtils.Editors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GUILabelAttribute : GUIMetaAttribute
    {
        public string Label { get; private set; }

        public GUILabelAttribute(string label)
        {
            Label = label;
        }
    }
}
