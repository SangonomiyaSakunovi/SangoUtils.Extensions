using System;

namespace SangoUtils.Bases_Unity
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RecognizableObjectAttribute : Attribute
    {
        public int RecognizableObjectGroupID { get; private set; }

        public RecognizableObjectAttribute(int recognizableObjectGroupID = 0)
        {
            RecognizableObjectGroupID = recognizableObjectGroupID;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class MarkerRecognizableObjectAttribute : RecognizableObjectAttribute
    {
        public MarkerRecognizableObjectAttribute(int markerObjectGroupID = 0) : base(markerObjectGroupID)
        {

        }
    }
}
