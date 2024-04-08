#if UNITY_EDITOR
using System;

namespace SangoUtils.Editors
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EditorObjectFieldAttribute : Attribute
    {

    }
}
#endif