using System;

namespace SangoUtils.Editors_Unity
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class GUIShowNativePropertyAttribute : SangoGUIDrawerSpecialAttribute
    {
    }
}
