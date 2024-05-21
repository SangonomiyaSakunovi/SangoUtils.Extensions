#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SangoUtils.CustomEditors_Unity
{
    public abstract class BaseEditorWindow : EditorWindow
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
                        EditorTextFieldAttribute attribute = (EditorTextFieldAttribute)field.GetCustomAttributes(typeof(EditorTextFieldAttribute), true)[0];
                        string name = string.IsNullOrEmpty(attribute.Text) ? ObjectNames.NicifyVariableName(field.Name) : attribute.Text;
                        
                        field.SetValue(this, EditorGUILayout.TextField(name, (string)field.GetValue(this)));
                    }
                    else if (field.GetCustomAttributes(typeof(EditorObjectFieldAttribute), true).Length > 0)
                    {
                        EditorObjectFieldAttribute attribute = (EditorObjectFieldAttribute)field.GetCustomAttributes(typeof(EditorObjectFieldAttribute), true)[0];
                        string name = string.IsNullOrEmpty(attribute.Text) ? ObjectNames.NicifyVariableName(field.Name) : attribute.Text;
                        
                        field.SetValue(this, EditorGUILayout.ObjectField(name, (GameObject)field.GetValue(this), typeof(GameObject), true));
                    }
                }
            }

            if (_methodInfos != null)
            {
                foreach (MethodInfo method in _methodInfos)
                {
                    if (method.GetCustomAttributes(typeof(EditorButtonAttribute), true).Length > 0)
                    {
                        EditorButtonAttribute attribute = (EditorButtonAttribute)method.GetCustomAttributes(typeof(EditorButtonAttribute), true)[0];
                        string name = string.IsNullOrEmpty(attribute.Text) ? ObjectNames.NicifyVariableName(method.Name) : attribute.Text;

                        if (GUILayout.Button(name))
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