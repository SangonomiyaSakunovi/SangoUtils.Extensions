using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SangoUtils_Extensions_UnityEngine.Core
{
    public static class UIElementExtensions_Button
    {
        public static void SetName(this Button button, string name)
        {
            button.name = name;
        }

        public static void SetActive(this Button button, bool isActive = true)
        {
            button.gameObject.SetActive(isActive);
        }

        public static void SetInteractable(this Button button, bool isInteractable = true)
        {
            button.interactable = isInteractable;
        }

        public static void AddListener_OnClick(this Button button, UnityAction<Button> callBack)
        {
            button.onClick.AddListener(() => callBack(button));
        }

        public static void AddListener_OnClick(this Button button, UnityAction callBack)
        {
            button.onClick.AddListener(callBack);
        }

        public static void RemoveAllListeners_OnClick(this Button button)
        {
            button.onClick.RemoveAllListeners();
        }

        public static void SetText(this Button button, string textStr)
        {
            TMP_Text text = button.transform.GetChild(0).GetComponent<TMP_Text>();
            if (text != null)
            {
                text.text = textStr;
            }
        }

        public static void SetColor(this Button button, int RValue, int GValue, int BValue, int AValue = 255)
        {
            Image image = button.GetComponent<Image>();
            if (image != null)
            {
                Color color = new Color(RValue / 255f, GValue / 255f, BValue / 255f);
                color.a = AValue / 255f;
                image.color = color;
            }
        }

        public static void SetSprite(this Button button, Sprite sprite)
        {
            Image image = button.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = sprite;
            }
        }
    }
}
