using UnityEditor;
using UnityEngine;

namespace SangoUtils.Editors_Unity
{
    public abstract class BaseGUIPropertyDrawer : PropertyDrawer
    {
        public sealed override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
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

        protected abstract void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label);

        sealed override public float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            bool visible = GUIDrawerPropertyUtils.IsVisible(property);
            if (!visible)
            {
                return 0.0f;
            }

            return GetPropertyHeight_Internal(property, label);
        }

        protected virtual float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, includeChildren: true);
        }

        protected float GetPropertyHeight(SerializedProperty property)
        {
            SangoGUIDrawerSpecialAttribute specialCaseAttribute = GUIDrawerPropertyUtils.GetAttribute<SangoGUIDrawerSpecialAttribute>(property);
            if (specialCaseAttribute != null)
            {
                return specialCaseAttribute.GetDrawer().GetPropertyHeight(property);
            }

            return EditorGUI.GetPropertyHeight(property, includeChildren: true);
        }

        public virtual float GetHelpBoxHeight()
        {
            return EditorGUIUtility.singleLineHeight * 2.0f;
        }

        public void DrawDefaultPropertyAndHelpBox(Rect rect, SerializedProperty property, string message, MessageType messageType)
        {
            float indentLength = GUIDrawerInspectorEditorUtils.GetIndentLength(rect);
            Rect helpBoxRect = new Rect(
                rect.x + indentLength,
                rect.y,
                rect.width - indentLength,
                GetHelpBoxHeight());

            GUIDrawerInspectorEditorUtils.HelpBox(helpBoxRect, message, MessageType.Warning, context: property.serializedObject.targetObject);

            Rect propertyRect = new Rect(
                rect.x,
                rect.y + GetHelpBoxHeight(),
                rect.width,
                GetPropertyHeight(property));

            EditorGUI.PropertyField(propertyRect, property, true);
        }
    }
}
