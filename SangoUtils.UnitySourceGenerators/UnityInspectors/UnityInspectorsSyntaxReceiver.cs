using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SangoUtils.UnitySourceGenerators.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SangoUtils.UnitySourceGenerators.UnityInspectors
{
    internal sealed class UnityInspectorsSyntaxReceiver : ISyntaxReceiver
    {
        public Dictionary<string, List<PropertyWorkItem>> CandidateWorkItems { get; } = new Dictionary<string, List<PropertyWorkItem>>();

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
                    CandidateWorkItems.Add(workItem.TypeName, new List<PropertyWorkItem> { workItem });
                }
            }
        }

        private static bool TryGetWorkItem(SyntaxNode syntaxNode, out PropertyWorkItem workItem)
        {
            if (syntaxNode is PropertyDeclarationSyntax propertyDeclarationSyntax
                && propertyDeclarationSyntax.AttributeLists.Count > 0)
            {
                var attributes = from attributeList in propertyDeclarationSyntax.AttributeLists
                                 from attribute in attributeList.Attributes
                                 select attribute;
                var item = new PropertyWorkItem(propertyDeclarationSyntax);
                foreach (var attribute in attributes)
                {
                    var attributeName = attribute.Name.ToString();
                    switch (attributeName)
                    {
                        case var name when name == UnityInspectorsSourceGenerator.UnityInspectorAttributeName ||
                                           name == UnityInspectorsSourceGenerator.UnityInspectorAttributeName + Def.Key_Attribute ||
                                           name == Def.Dom_Generateds + Def.Sym_Dot + UnityInspectorsSourceGenerator.UnityInspectorAttributeName ||
                                           name == Def.Dom_Generateds + Def.Sym_Dot + UnityInspectorsSourceGenerator.UnityInspectorAttributeName + Def.Key_Attribute:
                            item.SetIsExist(true);
                            break;
                    }
                }

                if (item.IsExist)
                {
                    var typeDeclarationSyntax = item.PropertyDeclarationSyntax.Parent as TypeDeclarationSyntax;
                    var typeName = new StringBuilder()
                        .Append(Def.Key_Partial)
                        .Append(Def.Fil_Space)
                        .Append(typeDeclarationSyntax.Keyword.ValueText)
                        .Append(Def.Fil_Space)
                        .Append(typeDeclarationSyntax.Identifier.ToString())
                        .Append(typeDeclarationSyntax.TypeParameterList)
                        .Append(Def.Fil_Space)
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
