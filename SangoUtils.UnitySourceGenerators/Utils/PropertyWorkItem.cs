using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SangoUtils.UnitySourceGenerators.Utils
{
    internal sealed class PropertyWorkItem
    {
        public readonly PropertyDeclarationSyntax PropertyDeclarationSyntax;
        public bool IsExist { get; private set; }
        public string TypeName { get; private set; }

        public PropertyWorkItem(PropertyDeclarationSyntax propertyDeclarationSyntax)
        {
            PropertyDeclarationSyntax = propertyDeclarationSyntax;
        }

        public void SetTypeName(string typeName)
        {
            TypeName = typeName;
        }

        public void SetIsExist(bool isExist)
        {
            IsExist = isExist;
        }
    }
}
