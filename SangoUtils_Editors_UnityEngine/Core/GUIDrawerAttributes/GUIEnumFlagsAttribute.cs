using System;

namespace SangoUtils.CustomEditors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GUIEnumFlagsAttribute : SangoGUIDrawerAttribute
    {
    }
}
