using UnityEngine.Events;
using UnityEngine.UI;

namespace SangoUtils.Extensions_Unity.Core
{
    public static class UIElementExtensions_Toggle
    {
        public static void SetName(this Toggle toggle, string name)
        {
            toggle.name = name;
        }

        public static void SetActive(this Toggle toggle, bool isActive = true)
        {
            toggle.gameObject.SetActive(isActive);
        }

        public static void SetToggleGroup(this Toggle toggle, ToggleGroup group)
        {
            toggle.group = group;
        }

        public static void SetInteractable(this Toggle toggle, bool isInteractable = true)
        {
            toggle.interactable = isInteractable;
        }

        public static void AddListener_OnValueChanged(this Toggle toggle, UnityAction<Toggle> callBack)
        {
            toggle.onValueChanged.AddListener((bool v) => callBack(toggle));
        }

        public static void RemoveAllListeners_OnValueChanged(this Toggle toggle)
        {
            toggle.onValueChanged.RemoveAllListeners();
        }
    }
}
