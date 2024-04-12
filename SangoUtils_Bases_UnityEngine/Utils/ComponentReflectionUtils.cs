using System.Collections.Generic;
using UnityEngine;

namespace SangoUtils.Bases_Unity.Utils
{
    internal static class ComponentReflectionUtils
    {
        public static List<Component> GetRelevantComponent(MonoBehaviour target)
        {
            List<Component> components = new List<Component>();
            var fields = MonobehaviorReflectionUtils.GetAllFields(target);
            if (fields != null)
            {
                foreach (var field in fields)
                {
                    if (field.GetCustomAttributes(typeof(RelevantComponentAttribute), true).Length > 0)
                    {
                        var component = field.GetValue(target) as Component;
                        if (component == null)
                        {
                            var fieldType = field.FieldType;
                            component = target.GetComponent(fieldType);
                        }
                        if (component != null)
                        {
                            components.Add(component);
                        }
                    }
                }
            }
            return components;
        }
    }
}
