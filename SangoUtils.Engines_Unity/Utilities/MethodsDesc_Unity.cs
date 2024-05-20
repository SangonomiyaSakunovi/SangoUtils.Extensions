namespace SangoUtils.Engines_Unity.Utilities
{
    public enum MethodParameterType
    {
        None = 0,
        Int = 1,
        Float = 2,
        String = 3,
        Object = 4
    }

    public class MethodDesc_BasicParam_1
    {
        public string name;
        public MethodParameterType type;
        public bool isOverload;
        public override string ToString()
        {
            return string.Format("{0}({1})", name, ParameterTypeToString());
        }

        public string ParameterTypeToString() => type switch
        {
            MethodParameterType.Int => "int",
            MethodParameterType.Float => "float",
            MethodParameterType.Object => "Object",
            MethodParameterType.String => "string",
            _ => string.Empty
        };

        public MethodDesc_BasicParam_1()
        {

        }

        public MethodDesc_BasicParam_1(string name, MethodParameterType type)
        {
            this.name = name;
            this.type = type;
        }

        public MethodDesc_BasicParam_1(string name, MethodParameterType type, bool isOverload)
        {
            this.name = name;
            this.type = type;
            this.isOverload = isOverload;
        }
    }
}
