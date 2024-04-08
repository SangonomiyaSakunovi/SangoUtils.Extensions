using UnityEditor;
using UnityEngine;

namespace SangoUtils.Editors_Unity
{
    [CustomPropertyDrawer(typeof(GUIHorizontalLineAttribute))]
    public class GUIHorizontalLineDecoratorDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            GUIHorizontalLineAttribute lineAttr = (GUIHorizontalLineAttribute)attribute;
            return EditorGUIUtility.singleLineHeight + lineAttr.Height;
        }

        public override void OnGUI(Rect position)
        {
            Rect rect = EditorGUI.IndentedRect(position);
            rect.y += EditorGUIUtility.singleLineHeight / 3.0f;
            GUIHorizontalLineAttribute lineAttr = (GUIHorizontalLineAttribute)attribute;
            GUIDrawerInspectorEditorUtils.HorizontalLine(rect, lineAttr.Height, lineAttr.Color.GetColor());
        }
    }
}
