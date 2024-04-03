using UnityEngine;
using UnityEngine.UI;

namespace SangoUtils.Extensions_Unity.Core
{
    public static class UIElementExtensions_Image
    {
        public static void SetSprite(this Image image, Sprite sprite)
        {
            image.sprite = sprite;
        }

        public static void SetSprite(this RawImage image, Texture texture)
        {
            image.texture = texture;
        }
    }
}
