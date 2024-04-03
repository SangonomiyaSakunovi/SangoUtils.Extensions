using System;

namespace SangoUtils.Editors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GUISliderAttribute : SangoGUIDrawerAttribute
    {
        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        public GUISliderAttribute(float minValue, float maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
