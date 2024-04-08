using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SangoUtils.Editors_Unity
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class GUIDrawerInspectorEditor : UnityEditor.Editor
    {
        private List<SerializedProperty> _serializedProperties = new List<SerializedProperty>();
        private IEnumerable<FieldInfo> _nonSerializedFields;
        private IEnumerable<PropertyInfo> _nativeProperties;
        private IEnumerable<MethodInfo> _methods;
        private Dictionary<string, GUIDrawerEditorPrefsUtils> _foldouts = new Dictionary<string, GUIDrawerEditorPrefsUtils>();

        protected virtual void OnEnable()
        {
            _nonSerializedFields = GUIDrawerReflectionUtils.GetAllFields(
                target, f => f.GetCustomAttributes(typeof(GUIShowNonSerializedFieldAttribute), true).Length > 0);

            _nativeProperties = GUIDrawerReflectionUtils.GetAllProperties(
                target, p => p.GetCustomAttributes(typeof(GUIShowNativePropertyAttribute), true).Length > 0);

            _methods = GUIDrawerReflectionUtils.GetAllMethods(
                target, m => m.GetCustomAttributes(typeof(GUIButtonAttribute), true).Length > 0);
        }

        protected virtual void OnDisable()
        {
            GUIReorderableListPropertyDrawer.Instance.ClearCache();
        }

        public override void OnInspectorGUI()
        {
            GetSerializedProperties(ref _serializedProperties);

            bool anyNaughtyAttribute = _serializedProperties.Any(p => GUIDrawerPropertyUtils.GetAttribute<ISangoGUIDrawerAttribute>(p) != null);
            if (!anyNaughtyAttribute)
            {
                DrawDefaultInspector();
            }
            else
            {
                DrawSerializedProperties();
            }

            DrawNonSerializedFields();
            DrawNativeProperties();
            DrawButtons();
        }

        protected void GetSerializedProperties(ref List<SerializedProperty> outSerializedProperties)
        {
            outSerializedProperties.Clear();
            using (var iterator = serializedObject.GetIterator())
            {
                if (iterator.NextVisible(true))
                {
                    do
                    {
                        outSerializedProperties.Add(serializedObject.FindProperty(iterator.name));
                    }
                    while (iterator.NextVisible(false));
                }
            }
        }

        protected void DrawSerializedProperties()
        {
            serializedObject.Update();

            // Draw non-grouped serialized properties
            foreach (var property in GetNonGroupedProperties(_serializedProperties))
            {
                if (property.name.Equals("m_Script", System.StringComparison.Ordinal))
                {
                    using (new EditorGUI.DisabledScope(disabled: true))
                    {
                        EditorGUILayout.PropertyField(property);
                    }
                }
                else
                {
                    GUIDrawerInspectorEditorUtils.PropertyField_Layout(property, includeChildren: true);
                }
            }

            // Draw grouped serialized properties
            foreach (var group in GetGroupedProperties(_serializedProperties))
            {
                IEnumerable<SerializedProperty> visibleProperties = group.Where(p => GUIDrawerPropertyUtils.IsVisible(p));
                if (!visibleProperties.Any())
                {
                    continue;
                }

                GUIDrawerInspectorEditorUtils.BeginBoxGroup_Layout(group.Key);
                foreach (var property in visibleProperties)
                {
                    GUIDrawerInspectorEditorUtils.PropertyField_Layout(property, includeChildren: true);
                }

                GUIDrawerInspectorEditorUtils.EndBoxGroup_Layout();
            }

            // Draw foldout serialized properties
            foreach (var group in GetFoldoutProperties(_serializedProperties))
            {
                IEnumerable<SerializedProperty> visibleProperties = group.Where(p => GUIDrawerPropertyUtils.IsVisible(p));
                if (!visibleProperties.Any())
                {
                    continue;
                }

                if (!_foldouts.ContainsKey(group.Key))
                {
                    _foldouts[group.Key] = new GUIDrawerEditorPrefsUtils($"{target.GetInstanceID()}.{group.Key}", false);
                }

                _foldouts[group.Key].Value = EditorGUILayout.Foldout(_foldouts[group.Key].Value, group.Key, true);
                if (_foldouts[group.Key].Value)
                {
                    foreach (var property in visibleProperties)
                    {
                        GUIDrawerInspectorEditorUtils.PropertyField_Layout(property, true);
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawNonSerializedFields(bool drawHeader = false)
        {
            if (_nonSerializedFields.Any())
            {
                if (drawHeader)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Non-Serialized Fields", GetHeaderGUIStyle());
                    GUIDrawerInspectorEditorUtils.HorizontalLine(
                        EditorGUILayout.GetControlRect(false), GUIHorizontalLineAttribute.DefaultHeight, GUIHorizontalLineAttribute.DefaultColor.GetColor());
                }

                foreach (var field in _nonSerializedFields)
                {
                    GUIDrawerInspectorEditorUtils.NonSerializedField_Layout(serializedObject.targetObject, field);
                }
            }
        }

        protected void DrawNativeProperties(bool drawHeader = false)
        {
            if (_nativeProperties.Any())
            {
                if (drawHeader)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Native Properties", GetHeaderGUIStyle());
                    GUIDrawerInspectorEditorUtils.HorizontalLine(
                        EditorGUILayout.GetControlRect(false), GUIHorizontalLineAttribute.DefaultHeight, GUIHorizontalLineAttribute.DefaultColor.GetColor());
                }

                foreach (var property in _nativeProperties)
                {
                    GUIDrawerInspectorEditorUtils.NativeProperty_Layout(serializedObject.targetObject, property);
                }
            }
        }

        protected void DrawButtons(bool drawHeader = false)
        {
            if (_methods.Any())
            {
                if (drawHeader)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Buttons", GetHeaderGUIStyle());
                    GUIDrawerInspectorEditorUtils.HorizontalLine(
                        EditorGUILayout.GetControlRect(false), GUIHorizontalLineAttribute.DefaultHeight, GUIHorizontalLineAttribute.DefaultColor.GetColor());
                }

                foreach (var method in _methods)
                {
                    GUIDrawerInspectorEditorUtils.Button(serializedObject.targetObject, method);
                }
            }
        }

        private static IEnumerable<SerializedProperty> GetNonGroupedProperties(IEnumerable<SerializedProperty> properties)
        {
            return properties.Where(p => GUIDrawerPropertyUtils.GetAttribute<ISangoGUIGroupAttribute>(p) == null);
        }

        private static IEnumerable<IGrouping<string, SerializedProperty>> GetGroupedProperties(IEnumerable<SerializedProperty> properties)
        {
            return properties
                .Where(p => GUIDrawerPropertyUtils.GetAttribute<GUIBoxGroupAttribute>(p) != null)
                .GroupBy(p => GUIDrawerPropertyUtils.GetAttribute<GUIBoxGroupAttribute>(p).Name);
        }

        private static IEnumerable<IGrouping<string, SerializedProperty>> GetFoldoutProperties(IEnumerable<SerializedProperty> properties)
        {
            return properties
                .Where(p => GUIDrawerPropertyUtils.GetAttribute<GUIFoldoutAttribute>(p) != null)
                .GroupBy(p => GUIDrawerPropertyUtils.GetAttribute<GUIFoldoutAttribute>(p).Name);
        }

        private static GUIStyle GetHeaderGUIStyle()
        {
            GUIStyle style = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.UpperCenter;

            return style;
        }
    }
}
