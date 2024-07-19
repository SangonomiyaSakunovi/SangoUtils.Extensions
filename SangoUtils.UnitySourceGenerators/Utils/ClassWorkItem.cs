using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SangoUtils.UnitySourceGenerators.Utils
{
    internal sealed class ClassWorkItem
    {
        public readonly ClassDeclarationSyntax ClassDeclarationSyntax;
        public bool IsExist { get; private set; }
        public string TypeName { get; private set; }

        public ClassWorkItem(ClassDeclarationSyntax classDeclarationSyntax)
        {
            ClassDeclarationSyntax = classDeclarationSyntax;
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
