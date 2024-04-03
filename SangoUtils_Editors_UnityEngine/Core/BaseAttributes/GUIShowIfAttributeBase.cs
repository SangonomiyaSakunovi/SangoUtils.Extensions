using System;

namespace SangoUtils.Editors_Unity
{
    public class GUIShowIfAttributeBase : GUIMetaAttribute
    {
        public string[] Conditions { get; private set; }
        public GUIConditionOperator ConditionOperator { get; private set; }
        public bool Inverted { get; protected set; }

        /// <summary>
        ///		If this not null, <see cref="Conditions"/>[0] is name of an enum variable.
        /// </summary>
        public Enum EnumValue { get; private set; }

        public GUIShowIfAttributeBase(string condition)
        {
            ConditionOperator = GUIConditionOperator.And;
            Conditions = new string[1] { condition };
        }

        public GUIShowIfAttributeBase(GUIConditionOperator conditionOperator, params string[] conditions)
        {
            ConditionOperator = conditionOperator;
            Conditions = conditions;
        }

        public GUIShowIfAttributeBase(string enumName, Enum enumValue)
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
