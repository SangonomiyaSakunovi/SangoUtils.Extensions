using System;

namespace SangoUtils.Editors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GUIFoldoutAttribute : GUIMetaAttribute, ISangoGUIGroupAttribute
    {
        public string Name { get; private set; }

        public GUIFoldoutAttribute(string name)
        {
            Name = name;
        }
    }
}
