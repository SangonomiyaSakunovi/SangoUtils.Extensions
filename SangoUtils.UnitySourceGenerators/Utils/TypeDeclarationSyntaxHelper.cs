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
                .Append(Def.Key_Partial)
                .Append(Def.Fil_Space)
                .Append(typeDeclarationSyntax.Keyword.ValueText)
                .Append(Def.Fil_Space)
                .Append(typeDeclarationSyntax.Identifier.ToString())
                .Append(typeDeclarationSyntax.TypeParameterList);

            foreach (var constraintClause in typeDeclarationSyntax.ConstraintClauses)
            {
                typeNameBuilder
                    .Append(Def.Fil_Space)
                    .Append(Def.Key_Where)
                    .Append(Def.Fil_Space);
                foreach (var childNode in constraintClause.ChildNodes())
                {
                    switch (childNode)
                    {
                        case IdentifierNameSyntax identifierNameSyntax:
                            typeNameBuilder
                                .Append(childNode)
                                .Append(Def.Fil_Space)
                                .Append(Def.Sym_Colon)
                                .Append(Def.Fil_Space);
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
