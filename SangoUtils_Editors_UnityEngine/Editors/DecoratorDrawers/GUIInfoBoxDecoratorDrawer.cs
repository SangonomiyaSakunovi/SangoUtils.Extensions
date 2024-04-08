using UnityEditor;
using UnityEngine;

namespace SangoUtils.Editors_Unity
{
    [CustomPropertyDrawer(typeof(GUIInfoBoxAttribute))]
    public class GUIInfoBoxDecoratorDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            return GetHelpBoxHeight();
        }

        public override void OnGUI(Rect rect)
        {
            GUIInfoBoxAttribute infoBoxAttribute = (GUIInfoBoxAttribute)attribute;

            float indentLength = GUIDrawerInspectorEditorUtils.GetIndentLength(rect);
            Rect infoBoxRect = new Rect(
                rect.x + indentLength,
                rect.y,
                rect.width - indentLength,
                GetHelpBoxHeight());

            DrawInfoBox(infoBoxRect, infoBoxAttribute.Text, infoBoxAttribute.Type);
        }

        private float GetHelpBoxHeight()
        {
            GUIInfoBoxAttribute infoBoxAttribute = (GUIInfoBoxAttribute)attribute;
            float minHeight = EditorGUIUtility.singleLineHeight * 2.0f;
            float desiredHeight = GUI.skin.box.CalcHeight(new GUIContent(infoBoxAttribute.Text), EditorGUIUtility.currentViewWidth);
            float height = Mathf.Max(minHeight, desiredHeight);

            return height;
        }

        private void DrawInfoBox(Rect rect, string infoText, GUIInfoBoxType infoBoxType)
        {
            MessageType messageType = MessageType.None;
            switch (infoBoxType)
            {
                case GUIInfoBoxType.Normal:
                    messageType = MessageType.Info;
                    break;

                case GUIInfoBoxType.Warning:
                    messageType = MessageType.Warning;
                    break;

                case GUIInfoBoxType.Error:
                    messageType = MessageType.Error;
                    break;
            }

            GUIDrawerInspectorEditorUtils.HelpBox(rect, infoText, messageType);
        }
    }
}
