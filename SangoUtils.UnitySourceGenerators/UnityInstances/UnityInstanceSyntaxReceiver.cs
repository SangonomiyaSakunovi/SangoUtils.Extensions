using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SangoUtils.UnitySourceGenerators.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SangoUtils.UnitySourceGenerators.UnityInstances
{
    internal sealed class UnityInstanceSyntaxReceiver : ISyntaxReceiver
    {
        public Dictionary<string, List<ClassWorkItem>> CandidateWorkItems { get; } = new Dictionary<string, List<ClassWorkItem>>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (TryGetWorkItem(syntaxNode, out var workItem))
            {
                if (CandidateWorkItems.ContainsKey(workItem.TypeName))
                {
                    CandidateWorkItems[workItem.TypeName].Add(workItem);
                }
                else
                {
                    CandidateWorkItems.Add(workItem.TypeName, new List<ClassWorkItem> { workItem });
                }
            }
        }

        private static bool TryGetWorkItem(SyntaxNode syntaxNode, out ClassWorkItem workItem)
        {
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
                && classDeclarationSyntax.AttributeLists.Count > 0)
            {
                var attributes = from attributeList in classDeclarationSyntax.AttributeLists
                                 from attribute in attributeList.Attributes
                                 select attribute;
                var item = new ClassWorkItem(classDeclarationSyntax);
                foreach (var attribute in attributes)
                {
                    var attributeName = attribute.Name.ToString();
                    switch (attributeName)
                    {
                        case var name when name == UnityInstanceSourceGenerator.UnityInstanceAttributeName ||
                                           name == UnityInstanceSourceGenerator.UnityInstanceAttributeName + "Attribute" ||
                                           name == Def.Dom_Generateds + "." + UnityInstanceSourceGenerator.UnityInstanceAttributeName ||
                                           name == Def.Dom_Generateds + "." + UnityInstanceSourceGenerator.UnityInstanceAttributeName + "Attribute":
                            item.SetIsExist(true);
                            break;
                    }
                }

                if (item.IsExist)
                {
                    var typeDeclarationSyntax = item.ClassDeclarationSyntax as TypeDeclarationSyntax;
                    var typeName = new StringBuilder()
                        .Append("partial ")
                        .Append(typeDeclarationSyntax.Keyword.ValueText)
                        .Append(" ")
                        .Append(typeDeclarationSyntax.Identifier.ToString())
                        .Append(typeDeclarationSyntax.TypeParameterList)
                        .Append(" ")
                        .Append(typeDeclarationSyntax.ConstraintClauses.ToString());
                    item.SetTypeName(typeName.ToString());
                    workItem = item;
                    return true;
                }
            }

            workItem = null;
            return false;
        }
    }
}
