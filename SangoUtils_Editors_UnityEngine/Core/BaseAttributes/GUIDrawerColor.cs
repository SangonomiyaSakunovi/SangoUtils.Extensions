using UnityEngine;

namespace SangoUtils.Editors_Unity
{
    public enum GUIDrawerColor
    {
        Clear,
        White,
        Black,
        Gray,
        Red,
        Pink,
        Orange,
        Yellow,
        Green,
        Blue,
        Indigo,
        Violet
    }

    public static class EColorExtensions
    {
        public static Color GetColor(this GUIDrawerColor color)
        {

            switch (color)
            {
                case GUIDrawerColor.Clear:
                    return new Color32(0, 0, 0, 0);
                case GUIDrawerColor.White:
                    return new Color32(255, 255, 255, 255);
                case GUIDrawerColor.Black:
                    return new Color32(0, 0, 0, 255);
                case GUIDrawerColor.Gray:
                    return new Color32(128, 128, 128, 255);
                case GUIDrawerColor.Red:
                    return new Color32(255, 0, 63, 255);
                case GUIDrawerColor.Pink:
                    return new Color32(255, 152, 203, 255);
                case GUIDrawerColor.Orange:
                    return new Color32(255, 128, 0, 255);
                case GUIDrawerColor.Yellow:
                    return new Color32(255, 211, 0, 255);
                case GUIDrawerColor.Green:
                    return new Color32(98, 200, 79, 255);
                case GUIDrawerColor.Blue:
                    return new Color32(0, 135, 189, 255);
                case GUIDrawerColor.Indigo:
                    return new Color32(75, 0, 130, 255);
                case GUIDrawerColor.Violet:
                    return new Color32(128, 0, 255, 255);
                default:
                    return new Color32(0, 0, 0, 255);
            }
        }
    }
}
