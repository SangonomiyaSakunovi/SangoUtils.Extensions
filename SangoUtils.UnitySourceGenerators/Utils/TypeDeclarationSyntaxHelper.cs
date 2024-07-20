using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace SangoUtils.UnitySourceGenerators.Utils
{
    internal static class TypeDeclarationSyntaxHelper
    {
        public static string WriteTypeName(in SemanticModel semanticModel, TypeDeclarationSyntax typeDeclarationSyntax)
        {
            var typeNameBuilder = new StringBuilder()
                .Append("partial ")
                .Append(typeDeclarationSyntax.Keyword.ValueText)
                .Append(" ")
                .Append(typeDeclarationSyntax.Identifier.ToString())
                .Append(typeDeclarationSyntax.TypeParameterList);

            foreach (var constraintClause in typeDeclarationSyntax.ConstraintClauses)
            {
                typeNameBuilder
                    .Append(" where ");
                foreach (var childNode in constraintClause.ChildNodes())
                {
                    switch (childNode)
                    {
                        case IdentifierNameSyntax identifierNameSyntax:
                            typeNameBuilder
                                .Append(childNode)
                                .Append(" : ");
                            break;
                        case TypeConstraintSyntax typeConstraintSyntax:
                            typeNameBuilder.Append(semanticModel.GetTypeInfo(typeConstraintSyntax.Type).Type.ToDisplayString());
                            break;
                    }
                }
            }

            return typeNameBuilder.ToString();
        }
    }
}
