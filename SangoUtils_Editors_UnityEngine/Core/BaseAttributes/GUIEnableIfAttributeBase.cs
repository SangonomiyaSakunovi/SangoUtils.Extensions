using System;

namespace SangoUtils.CustomEditors_Unity
{
    public abstract class GUIEnableIfAttributeBase : GUIMetaAttribute
    {
        public string[] Conditions { get; private set; }
        public GUIConditionOperator ConditionOperator { get; private set; }
        public bool Inverted { get; protected set; }

        /// <summary>
        ///		If this not null, <see cref="Conditions"/>[0] is name of an enum variable.
        /// </summary>
        public Enum EnumValue { get; private set; }

        public GUIEnableIfAttributeBase(string condition)
        {
            ConditionOperator = GUIConditionOperator.And;
            Conditions = new string[1] { condition };
        }

        public GUIEnableIfAttributeBase(GUIConditionOperator conditionOperator, params string[] conditions)
        {
            ConditionOperator = conditionOperator;
            Conditions = conditions;
        }

        public GUIEnableIfAttributeBase(string enumName, Enum enumValue)
            : this(enumName)
        {
            if (enumValue == null)
            {
                throw new ArgumentNullException(nameof(enumValue), "This parameter must be an enum value.");
            }

            EnumValue = enumValue;
        }
    }
}
