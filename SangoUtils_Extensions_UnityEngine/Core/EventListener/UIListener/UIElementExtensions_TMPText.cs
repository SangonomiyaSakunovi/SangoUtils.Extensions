using TMPro;

namespace SangoUtils.Extensions_Unity.Core
{
    public static class UIElementExtensions_TMPText
    {
        public static void SetName(this TMP_Text text, string name)
        {
            text.name = name;
        }

        public static void SetActive(this TMP_Text text, bool isActive = true)
        {
            text.gameObject.SetActive(isActive);
        }

        public static void SetText(this TMP_Text text, string textStr)
        {
            text.text = textStr;
        }
    }
}
