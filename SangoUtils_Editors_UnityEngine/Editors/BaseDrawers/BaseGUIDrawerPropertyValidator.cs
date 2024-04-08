using UnityEditor;

namespace SangoUtils.Editors_Unity
{
    public abstract class BaseGUIDrawerPropertyValidator
    {
        public abstract void ValidateProperty(SerializedProperty property);
    }
}
