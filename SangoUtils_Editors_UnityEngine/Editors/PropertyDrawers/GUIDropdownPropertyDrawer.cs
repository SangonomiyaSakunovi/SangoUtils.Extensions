using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SangoUtils.Editors_Unity
{
    [CustomPropertyDrawer(typeof(GUIDropdownAttribute))]
    public class GUIDropdownPropertyDrawer : BaseGUIPropertyDrawer
    {
        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            GUIDropdownAttribute dropdownAttribute = (GUIDropdownAttribute)attribute;
            object values = GetValues(property, dropdownAttribute.ValuesName);
            FieldInfo fieldInfo = GUIDrawerReflectionUtils.GetField(GUIDrawerPropertyUtils.GetTargetObjectWithProperty(property), property.name);

            float propertyHeight = AreValuesValid(values, fieldInfo)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();

            return propertyHeight;
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            GUIDropdownAttribute dropdownAttribute = (GUIDropdownAttribute)attribute;
            object target = GUIDrawerPropertyUtils.GetTargetObjectWithProperty(property);

            object valuesObject = GetValues(property, dropdownAttribute.ValuesName);
            FieldInfo dropdownField = GUIDrawerReflectionUtils.GetField(target, property.name);

            if (AreValuesValid(valuesObject, dropdownField))
            {
                if (valuesObject is IList && dropdownField.FieldType == GetElementType(valuesObject))
                {
                    // Selected value
                    object selectedValue = dropdownField.GetValue(target);

                    // Values and display options
                    IList valuesList = (IList)valuesObject;
                    object[] values = new object[valuesList.Count];
                    string[] displayOptions = new string[valuesList.Count];

                    for (int i = 0; i < values.Length; i++)
                    {
                        object value = valuesList[i];
                        values[i] = value;
                        displayOptions[i] = value == null ? "<null>" : value.ToString();
                    }

                    // Selected value index
                    int selectedValueIndex = Array.IndexOf(values, selectedValue);
                    if (selectedValueIndex < 0)
                    {
                        selectedValueIndex = 0;
                    }

                    GUIDrawerInspectorEditorUtils.Dropdown(
                        rect, property.serializedObject, target, dropdownField, label.text, selectedValueIndex, values, displayOptions);
                }
                else if (valuesObject is IGUIDropdownList)
                {
                    // Current value
                    object selectedValue = dropdownField.GetValue(target);

                    // Current value index, values and display options
                    int index = -1;
                    int selectedValueIndex = -1;
                    List<object> values = new List<object>();
                    List<string> displayOptions = new List<string>();
                    IGUIDropdownList dropdown = (IGUIDropdownList)valuesObject;

                    using (IEnumerator<KeyValuePair<string, object>> dropdownEnumerator = dropdown.GetEnumerator())
                    {
                        while (dropdownEnumerator.MoveNext())
                        {
                            index++;

                            KeyValuePair<string, object> current = dropdownEnumerator.Current;
                            if (current.Value?.Equals(selectedValue) == true)
                            {
                                selectedValueIndex = index;
                            }

                            values.Add(current.Value);

                            if (current.Key == null)
                            {
                                displayOptions.Add("<null>");
                            }
                            else if (string.IsNullOrWhiteSpace(current.Key))
                            {
                                displayOptions.Add("<empty>");
                            }
                            else
                            {
                                displayOptions.Add(current.Key);
                            }
                        }
                    }

                    if (selectedValueIndex < 0)
                    {
                        selectedValueIndex = 0;
                    }

                    GUIDrawerInspectorEditorUtils.Dropdown(
                        rect, property.serializedObject, target, dropdownField, label.text, selectedValueIndex, values.ToArray(), displayOptions.ToArray());
                }
            }
            else
            {
                string message = string.Format("Invalid values with name '{0}' provided to '{1}'. Either the values name is incorrect or the types of the target field and the values field/property/method don't match",
                    dropdownAttribute.ValuesName, dropdownAttribute.GetType().Name);

                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }

        private object GetValues(SerializedProperty property, string valuesName)
        {
            object target = GUIDrawerPropertyUtils.GetTargetObjectWithProperty(property);

            FieldInfo valuesFieldInfo = GUIDrawerReflectionUtils.GetField(target, valuesName);
            if (valuesFieldInfo != null)
            {
                return valuesFieldInfo.GetValue(target);
            }

            PropertyInfo valuesPropertyInfo = GUIDrawerReflectionUtils.GetProperty(target, valuesName);
            if (valuesPropertyInfo != null)
            {
                return valuesPropertyInfo.GetValue(target);
            }

            MethodInfo methodValuesInfo = GUIDrawerReflectionUtils.GetMethod(target, valuesName);
            if (methodValuesInfo != null &&
                methodValuesInfo.ReturnType != typeof(void) &&
                methodValuesInfo.GetParameters().Length == 0)
            {
                return methodValuesInfo.Invoke(target, null);
            }

            return null;
        }

        private bool AreValuesValid(object values, FieldInfo dropdownField)
        {
            if (values == null || dropdownField == null)
            {
                return false;
            }

            if ((values is IList && dropdownField.FieldType == GetElementType(values)) ||
                (values is IGUIDropdownList))
            {
                return true;
            }

            return false;
        }

        private Type GetElementType(object values)
        {
            Type valuesType = values.GetType();
            Type elementType = GUIDrawerReflectionUtils.GetListElementType(valuesType);

            return elementType;
        }
    }
}
