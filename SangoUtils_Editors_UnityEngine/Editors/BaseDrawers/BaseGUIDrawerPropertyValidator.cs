using UnityEditor;

namespace SangoUtils.CustomEditors_Unity
{
    public abstract class BaseGUIDrawerPropertyValidator
    {
        public abstract void ValidateProperty(SerializedProperty property);
    }
}
