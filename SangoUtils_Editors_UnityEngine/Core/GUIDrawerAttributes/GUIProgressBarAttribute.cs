using System;

namespace SangoUtils.CustomEditors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GUIProgressBarAttribute : SangoGUIDrawerAttribute
    {
        public string Name { get; private set; }
        public float MaxValue { get; set; }
        public string MaxValueName { get; private set; }
        public GUIDrawerColor Color { get; private set; }

        public GUIProgressBarAttribute(string name, float maxValue, GUIDrawerColor color = GUIDrawerColor.Blue)
        {
            Name = name;
            MaxValue = maxValue;
            Color = color;
        }

        public GUIProgressBarAttribute(string name, string maxValueName, GUIDrawerColor color = GUIDrawerColor.Blue)
        {
            Name = name;
            MaxValueName = maxValueName;
            Color = color;
        }

        public GUIProgressBarAttribute(float maxValue, GUIDrawerColor color = GUIDrawerColor.Blue)
            : this("", maxValue, color)
        {
        }

        public GUIProgressBarAttribute(string maxValueName, GUIDrawerColor color = GUIDrawerColor.Blue)
            : this("", maxValueName, color)
        {
        }
    }
}
