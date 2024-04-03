using System;

namespace SangoUtils.Editors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GUIMinValueAttribute : GUIValidatorAttribute
    {
        public float MinValue { get; private set; }

        public GUIMinValueAttribute(float minValue)
        {
            MinValue = minValue;
        }

        public GUIMinValueAttribute(int minValue)
        {
            MinValue = minValue;
        }
    }
}
