using System;

namespace SangoUtils.CustomEditors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class GUIOnValueChangedAttribute : GUIMetaAttribute
    {
        public string CallbackName { get; private set; }

        public GUIOnValueChangedAttribute(string callbackName)
        {
            CallbackName = callbackName;
        }
    }
}
