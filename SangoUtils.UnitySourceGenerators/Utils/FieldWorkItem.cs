using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SangoUtils.UnitySourceGenerators.Utils
{
    internal sealed class FieldWorkItem
    {
        public readonly FieldDeclarationSyntax FieldDeclarationSyntax;
        public bool IsExist { get; private set; }
        public string TypeName { get; private set; }

        public FieldWorkItem(FieldDeclarationSyntax fieldDeclarationSyntax)
        {
            FieldDeclarationSyntax = fieldDeclarationSyntax;
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
