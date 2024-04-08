#if UNITY_EDITOR
using System;

namespace SangoUtils.Editors
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class EditorButtonAttribute : Attribute
    {

    }
}
#endif