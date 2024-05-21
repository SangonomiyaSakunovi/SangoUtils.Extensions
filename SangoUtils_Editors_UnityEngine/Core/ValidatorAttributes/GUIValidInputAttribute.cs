using System;

namespace SangoUtils.CustomEditors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GUIValidInputAttribute : GUIValidatorAttribute
    {
        public string CallbackName { get; private set; }
        public string Message { get; private set; }

        public GUIValidInputAttribute(string callbackName, string message = null)
        {
            CallbackName = callbackName;
            Message = message;
        }
    }
}
