using System;
using System.Collections;
using System.Collections.Generic;

namespace SangoUtils.Editors_Unity
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GUIDropdownAttribute : SangoGUIDrawerAttribute
    {
        public string ValuesName { get; private set; }

        public GUIDropdownAttribute(string valuesName)
        {
            ValuesName = valuesName;
        }
    }

    public interface IGUIDropdownList : IEnumerable<KeyValuePair<string, object>>
    {
    }

    public class GUIDropdownList<T> : IGUIDropdownList
    {
        private List<KeyValuePair<string, object>> _values;

        public GUIDropdownList()
        {
            _values = new List<KeyValuePair<string, object>>();
        }

        public void Add(string displayName, T value)
        {
            _values.Add(new KeyValuePair<string, object>(displayName, value));
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static explicit operator GUIDropdownList<object>(GUIDropdownList<T> target)
        {
            GUIDropdownList<object> result = new GUIDropdownList<object>();
            foreach (var kvp in target)
            {
                result.Add(kvp.Key, kvp.Value);
            }

            return result;
        }
    }
}
