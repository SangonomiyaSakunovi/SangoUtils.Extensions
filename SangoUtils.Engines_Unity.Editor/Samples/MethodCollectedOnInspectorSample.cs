using SangoUtils.Engines_Unity.Utilities;
using UnityEngine;

namespace SangoUtils.EngineEditors_Unity.Samples
{
    internal class MethodCollectedOnInspectorSample : MonoBehaviour
    {
        [SerializeField]
        public string method0;
        [SerializeField]
        public MethodParameterType methodParameterType0;
        [SerializeField]
        public int Int0;
        [SerializeField]
        public string String0;
        [SerializeField]
        public float Float0;
        [SerializeField]
        public UnityEngine.Object Object0;

        private static object ArgConvert0(MethodCollectedOnInspectorSample message) => message.methodParameterType0 switch
        {
            MethodParameterType.Int => message.Int0,
            MethodParameterType.Float => message.Float0,
            MethodParameterType.String => message.String0,
            MethodParameterType.Object => message.Object0,
            _ => null
        };

        private void SampleMethod()
        {
            SendMessage(method0, ArgConvert0(this));
        }
    }
}
