using System;
using UnityEngine;

namespace SangoUtils.Editors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GUICurveRangeAttribute : SangoGUIDrawerAttribute
    {
        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }
        public GUIDrawerColor Color { get; private set; }

        public GUICurveRangeAttribute(Vector2 min, Vector2 max, GUIDrawerColor color = GUIDrawerColor.Clear)
        {
            Min = min;
            Max = max;
            Color = color;
        }

        public GUICurveRangeAttribute(GUIDrawerColor color)
            : this(Vector2.zero, Vector2.one, color)
        {
        }

        public GUICurveRangeAttribute(float minX, float minY, float maxX, float maxY, GUIDrawerColor color = GUIDrawerColor.Clear)
            : this(new Vector2(minX, minY), new Vector2(maxX, maxY), color)
        {
        }
    }
}
