using System;

namespace SangoUtils.CustomEditors_Unity
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class GUIDisableIfAttribute : GUIEnableIfAttributeBase
    {
        public GUIDisableIfAttribute(string condition)
            : base(condition)
        {
            Inverted = true;
        }

        public GUIDisableIfAttribute(GUIConditionOperator conditionOperator, params string[] conditions)
            : base(conditionOperator, conditions)
        {
            Inverted = true;
        }

        public GUIDisableIfAttribute(string enumName, object enumValue)
            : base(enumName, enumValue as Enum)
        {
            Inverted = true;
        }
    }
}
