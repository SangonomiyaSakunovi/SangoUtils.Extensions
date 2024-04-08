using UnityEditor;

namespace SangoUtils.Editors_Unity
{
    internal class GUIDrawerEditorPrefsUtils
    {
        private bool _value;
        private string _name;

        public bool Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == value)
                {
                    return;
                }

                _value = value;
                EditorPrefs.SetBool(_name, value);
            }
        }

        public GUIDrawerEditorPrefsUtils(string name, bool value)
        {
            _name = name;
            _value = EditorPrefs.GetBool(name, value);
        }
    }
}
