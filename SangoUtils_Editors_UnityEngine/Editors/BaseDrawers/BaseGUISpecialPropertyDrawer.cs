using UnityEditor;
using UnityEngine;

namespace SangoUtils.CustomEditors_Unity
{
    public abstract class BaseGUISpecialPropertyDrawer
    {
        public void OnGUI(Rect rect, SerializedProperty property)
        {
            // Check if visible
            bool visible = GUIDrawerPropertyUtils.IsVisible(property);
            if (!visible)
            {
                return;
            }

            // Validate
            GUIValidatorAttribute[] validatorAttributes = GUIDrawerPropertyUtils.GetAttributes<GUIValidatorAttribute>(property);
            foreach (var validatorAttribute in validatorAttributes)
            {
                validatorAttribute.GetValidator().ValidateProperty(property);
            }

            // Check if enabled and draw
            EditorGUI.BeginChangeCheck();
            bool enabled = GUIDrawerPropertyUtils.IsEnabled(property);

            using (new EditorGUI.DisabledScope(disabled: !enabled))
            {
                OnGUI_Internal(rect, property, GUIDrawerPropertyUtils.GetLabel(property));
            }

            // Call OnValueChanged callbacks
            if (EditorGUI.EndChangeCheck())
            {
                GUIDrawerPropertyUtils.CallOnValueChangedCallbacks(property);
            }
        }

        public float GetPropertyHeight(SerializedProperty property)
        {
            return GetPropertyHeight_Internal(property);
        }

        protected abstract void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label);
        protected abstract float GetPropertyHeight_Internal(SerializedProperty property);
    }
}
