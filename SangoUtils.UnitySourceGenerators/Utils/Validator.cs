using System;
using System.Collections.Generic;
using System.Text;

namespace SangoUtils.UnitySourceGenerators.Utils
{
    internal static class Validator
    {
        public static bool IsMouduleNameStartsValid(string moduleName)
        {
            if (moduleName.StartsWith("UnityEngine.")) return false;
            if (moduleName.StartsWith("UnityEditor.")) return false;
            if (moduleName.StartsWith("Unity.")) return false;
            if (moduleName.StartsWith("PPv2URPConverters.")) return false;

            return true;
        }
    }
}
