using System;

namespace SangoUtils.Editors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class GUIHorizontalLineAttribute : SangoGUIDrawerAttribute
    {
        public const float DefaultHeight = 2.0f;
        public const GUIDrawerColor DefaultColor = GUIDrawerColor.Gray;

        public float Height { get; private set; }
        public GUIDrawerColor Color { get; private set; }

        public GUIHorizontalLineAttribute(float height = DefaultHeight, GUIDrawerColor color = DefaultColor)
        {
            Height = height;
            Color = color;
        }
    }
}
