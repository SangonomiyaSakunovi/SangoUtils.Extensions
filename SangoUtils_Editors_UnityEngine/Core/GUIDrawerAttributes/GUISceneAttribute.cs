using SangoUtils.Editors_Unity;
using System;

namespace SangoUtils.Editors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GUISceneAttribute : SangoGUIDrawerAttribute
    {
    }
}
