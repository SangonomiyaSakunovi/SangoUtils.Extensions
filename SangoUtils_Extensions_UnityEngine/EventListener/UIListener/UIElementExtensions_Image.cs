using UnityEngine;
using UnityEngine.UI;

namespace SangoUtils_Extensions_UnityEngine.Core
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
