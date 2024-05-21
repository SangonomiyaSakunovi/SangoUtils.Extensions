using UnityEditor;
using UnityEngine;

namespace SangoUtils.CustomEditors_Unity
{
    [CustomPropertyDrawer(typeof(GUIAllowNestingAttribute))]
    public class GUIAllowNestingPropertyDrawer : BaseGUIPropertyDrawer
    {
        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            EditorGUI.PropertyField(rect, property, label, true);
            EditorGUI.EndProperty();
        }
    }
}
