using System;

namespace SangoUtils.CustomEditors_Unity
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class GUIShowNativePropertyAttribute : SangoGUIDrawerSpecialAttribute
    {
    }
}
