using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SangoUtils.CustomEditors_Unity
{
    internal static class GUIDrawerInspectorButtonUtils
    {
        public static bool IsEnabled(UnityEngine.Object target, MethodInfo method)
        {
            GUIEnableIfAttributeBase enableIfAttribute = method.GetCustomAttribute<GUIEnableIfAttributeBase>();
            if (enableIfAttribute == null)
            {
                return true;
            }

            List<bool> conditionValues = GUIDrawerPropertyUtils.GetConditionValues(target, enableIfAttribute.Conditions);
            if (conditionValues.Count > 0)
            {
                bool enabled = GUIDrawerPropertyUtils.GetConditionsFlag(conditionValues, enableIfAttribute.ConditionOperator, enableIfAttribute.Inverted);
                return enabled;
            }
            else
            {
                string message = enableIfAttribute.GetType().Name + " needs a valid boolean condition field, property or method name to work";
                Debug.LogWarning(message, target);

                return false;
            }
        }

        public static bool IsVisible(UnityEngine.Object target, MethodInfo method)
        {
            GUIShowIfAttributeBase showIfAttribute = method.GetCustomAttribute<GUIShowIfAttributeBase>();
            if (showIfAttribute == null)
            {
                return true;
            }

            List<bool> conditionValues = GUIDrawerPropertyUtils.GetConditionValues(target, showIfAttribute.Conditions);
            if (conditionValues.Count > 0)
            {
                bool enabled = GUIDrawerPropertyUtils.GetConditionsFlag(conditionValues, showIfAttribute.ConditionOperator, showIfAttribute.Inverted);
                return enabled;
            }
            else
            {
                string message = showIfAttribute.GetType().Name + " needs a valid boolean condition field, property or method name to work";
                Debug.LogWarning(message, target);

                return false;
            }
        }
    }
}
