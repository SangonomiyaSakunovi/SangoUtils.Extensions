using System;

namespace SangoUtils.Editors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GUIBoxGroupAttribute : GUIMetaAttribute, ISangoGUIGroupAttribute
    {
        public string Name { get; private set; }

        public GUIBoxGroupAttribute(string name = "")
        {
            Name = name;
        }
    }
}
