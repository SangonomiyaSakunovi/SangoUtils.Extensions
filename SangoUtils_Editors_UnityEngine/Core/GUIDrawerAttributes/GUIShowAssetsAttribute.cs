using System;

namespace SangoUtils.CustomEditors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GUIShowAssetsAttribute : SangoGUIDrawerAttribute
    {
        public const int DefaultWidth = 64;
        public const int DefaultHeight = 64;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public GUIShowAssetsAttribute(int width = DefaultWidth, int height = DefaultHeight)
        {
            Width = width;
            Height = height;
        }
    }
}
