using System;

namespace SangoUtils.CustomEditors_Unity
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class GUIHideIfAttribute : GUIShowIfAttributeBase
    {
        public GUIHideIfAttribute(string condition)
            : base(condition)
        {
            Inverted = true;
        }

        public GUIHideIfAttribute(GUIConditionOperator conditionOperator, params string[] conditions)
            : base(conditionOperator, conditions)
        {
            Inverted = true;
        }

        public GUIHideIfAttribute(string enumName, object enumValue)
            : base(enumName, enumValue as Enum)
        {
            Inverted = true;
        }
    }
}
