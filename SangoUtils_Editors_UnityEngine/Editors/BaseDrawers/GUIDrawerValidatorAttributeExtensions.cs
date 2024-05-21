using System;
using System.Collections.Generic;

namespace SangoUtils.CustomEditors_Unity
{
    public static class GUIDrawerValidatorAttributeExtensions
    {
        private static Dictionary<Type, BaseGUIDrawerPropertyValidator> _validatorsByAttributeType;

        static GUIDrawerValidatorAttributeExtensions()
        {
            _validatorsByAttributeType = new Dictionary<Type, BaseGUIDrawerPropertyValidator>();
            _validatorsByAttributeType[typeof(GUIMinValueAttribute)] = new GUIDrawerMinValuePropertyValidator();
            _validatorsByAttributeType[typeof(GUIMaxValueAttribute)] = new GUIDrawerMaxValuePropertyValidator();
            _validatorsByAttributeType[typeof(GUIRequiredAttribute)] = new GUIDrawerRequiredPropertyValidator();
            _validatorsByAttributeType[typeof(GUIValidInputAttribute)] = new GUIDrawerValidateInputPropertyValidator();
        }

        internal static BaseGUIDrawerPropertyValidator GetValidator(this GUIValidatorAttribute attr)
        {
            BaseGUIDrawerPropertyValidator validator;
            if (_validatorsByAttributeType.TryGetValue(attr.GetType(), out validator))
            {
                return validator;
            }
            else
            {
                return null;
            }
        }
    }
}
