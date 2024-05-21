using UnityEditor;

namespace SangoUtils.CustomEditors_Unity
{
    internal class GUIDrawerRequiredPropertyValidator : BaseGUIDrawerPropertyValidator
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            GUIRequiredAttribute requiredAttribute = GUIDrawerPropertyUtils.GetAttribute<GUIRequiredAttribute>(property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == null)
                {
                    string errorMessage = property.name + " is required";
                    if (!string.IsNullOrEmpty(requiredAttribute.Message))
                    {
                        errorMessage = requiredAttribute.Message;
                    }

                    GUIDrawerInspectorEditorUtils.HelpBox_Layout(errorMessage, MessageType.Error, context: property.serializedObject.targetObject);
                }
            }
            else
            {
                string warning = requiredAttribute.GetType().Name + " works only on reference types";
                GUIDrawerInspectorEditorUtils.HelpBox_Layout(warning, MessageType.Warning, context: property.serializedObject.targetObject);
            }
        }
    }
}
