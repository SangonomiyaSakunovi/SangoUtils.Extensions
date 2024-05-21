using System;

namespace SangoUtils.CustomEditors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GUIRequiredAttribute : GUIValidatorAttribute
    {
        public string Message { get; private set; }

        public GUIRequiredAttribute(string message = null)
        {
            Message = message;
        }
    }
}
