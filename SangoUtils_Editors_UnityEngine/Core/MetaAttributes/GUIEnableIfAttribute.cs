using System;

namespace SangoUtils.CustomEditors_Unity
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class GUIEnableIfAttribute : GUIEnableIfAttributeBase
    {
        public GUIEnableIfAttribute(string condition)
            : base(condition)
        {
            Inverted = false;
        }

        public GUIEnableIfAttribute(GUIConditionOperator conditionOperator, params string[] conditions)
            : base(conditionOperator, conditions)
        {
            Inverted = false;
        }

        public GUIEnableIfAttribute(string enumName, object enumValue)
            : base(enumName, enumValue as Enum)
        {
            Inverted = false;
        }
    }
}
