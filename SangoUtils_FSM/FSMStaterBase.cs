using System.Collections.Generic;

namespace SangoUtils.FSMs
{
    public class FSMStaterBase
    {
        protected Dictionary<string, object> _blackboard = new Dictionary<string, object>();

        public void SetBlackboardValue(string key, object value)
        {
            if (_blackboard.ContainsKey(key))
            {
                _blackboard[key] = value;
            }
            else
            {
                _blackboard.Add(key, value);
            }
        }

        public object? GetBlackboardValue(string key)
        {
            if (_blackboard.TryGetValue(key, out object value))
            {
                return value;
            }
            return null;
        }
    }
}