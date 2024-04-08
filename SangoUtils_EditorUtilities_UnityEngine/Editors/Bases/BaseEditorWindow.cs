#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SangoUtils.Editors
{
    public class BaseEditorWindow : EditorWindow
    {
        private FieldInfo[]? _fieldInfos;
        private MethodInfo[]? _methodInfos;

        private void OnEnable()
        {
            _fieldInfos = EditorWindowReflectionUtils.GetAllFields(this);
            _methodInfos = EditorWindowReflectionUtils.GetAllMethods(this);
        }

        private void OnGUI()
        {
            if (_fieldInfos != null)
            {
                foreach (FieldInfo field in _fieldInfos)
                {
                    if (field.GetCustomAttributes(typeof(EditorTextFieldAttribute), true).Length > 0)
                    {
                        field.SetValue(this, EditorGUILayout.TextField(field.Name, (string)field.GetValue(this)));
                    }
                    else if (field.GetCustomAttributes(typeof(EditorObjectFieldAttribute), true).Length > 0)
                    {
                        field.SetValue(this, EditorGUILayout.ObjectField(field.Name, (GameObject)field.GetValue(this), typeof(GameObject), true));
                    }
                }
            }

            if (_methodInfos != null)
            {
                foreach (MethodInfo method in _methodInfos)
                {
                    if (method.GetCustomAttributes(typeof(EditorButtonAttribute), true).Length > 0)
                    {
                        if (GUILayout.Button(method.Name))
                        {
                            method.Invoke(this, null);
                        }
                    }
                }
            }
        }
    }
}
#endif