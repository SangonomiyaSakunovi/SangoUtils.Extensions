using System;
using System.Collections.Generic;

namespace SangoUtils.Editors_Unity
{
    public static class GUIDrawerSpecialAttributeExtensions
    {
        private static Dictionary<Type, BaseGUISpecialPropertyDrawer> _drawersByAttributeType;

        static GUIDrawerSpecialAttributeExtensions()
        {
            _drawersByAttributeType = new Dictionary<Type, BaseGUISpecialPropertyDrawer>();
            _drawersByAttributeType[typeof(GUIReordListAttribute)] = GUIReorderableListPropertyDrawer.Instance;
        }

        public static BaseGUISpecialPropertyDrawer GetDrawer(this SangoGUIDrawerSpecialAttribute attr)
        {
            BaseGUISpecialPropertyDrawer drawer;
            if (_drawersByAttributeType.TryGetValue(attr.GetType(), out drawer))
            {
                return drawer;
            }
            else
            {
                return null;
            }
        }
    }
}
