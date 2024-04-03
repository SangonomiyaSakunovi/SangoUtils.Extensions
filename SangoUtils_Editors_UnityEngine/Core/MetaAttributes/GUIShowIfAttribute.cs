using System;

namespace SangoUtils.Editors_Unity
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class GUIShowIfAttribute : GUIShowIfAttributeBase
    {
        public GUIShowIfAttribute(string condition)
            : base(condition)
        {
            Inverted = false;
        }

        public GUIShowIfAttribute(GUIConditionOperator conditionOperator, params string[] conditions)
            : base(conditionOperator, conditions)
        {
            Inverted = false;
        }

        public GUIShowIfAttribute(string enumName, object enumValue)
            : base(enumName, enumValue as Enum)
        {
            Inverted = false;
        }
    }
}
