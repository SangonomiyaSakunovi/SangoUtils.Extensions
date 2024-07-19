using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SangoUtils.UnitySourceGenerators.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SangoUtils.UnitySourceGenerators.AnimatorViews
{
    //internal sealed class AnimatorViewSyntaxReceiver : ISyntaxReceiver
    //{
    //    public Dictionary<string, List<ClassWorkItem>> CandidateWorkItems { get; } = new Dictionary<string, List<ClassWorkItem>>();

    //    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    //    {
    //        if (TryGetWorkItem(syntaxNode, out var workItem))
    //        {
    //            CandidateWorkItems[workItem.TypeName].Add(workItem);
    //        }
    //        else
    //        {
    //            CandidateWorkItems.Add(workItem.TypeName, new List<ClassWorkItem> { workItem });
    //        }
    //    }

    //    private static bool TryGetWorkItem(SyntaxNode syntaxNode, out ClassWorkItem workItem)
    //    {
    //        if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
    //            && classDeclarationSyntax.AttributeLists.Count > 0)
    //        {
    //            var attributes = from attributeList in classDeclarationSyntax.AttributeLists
    //                             from attribute in attributeList.Attributes
    //                             select attribute;
    //            var item = new ClassWorkItem(classDeclarationSyntax);
    //            foreach (var attibute in attributes)
    //            {
    //                var attributeName = attibute.Name.ToString();
    //                switch (attributeName)
    //                {
    //                    case var name when name == AnimatorParameterViewSourceGenerator.AnimatorParameterViewAttributeName ||
    //                                       name == AnimatorParameterViewSourceGenerator.AnimatorParameterViewAttributeName + Def.Key_Attribute ||
    //                                       name == Def.Dom_Generateds + Def.Sym_Dot + AnimatorParameterViewSourceGenerator.AnimatorParameterViewAttributeName ||
    //                                       name == Def.Dom_Generateds + Def.Sym_Dot + AnimatorParameterViewSourceGenerator.AnimatorParameterViewAttributeName + Def.Key_Attribute:
    //                        item.SetIsExist(true);
    //                        break;
    //                }

    //                if (item.IsExist)
    //                {
    //                    var typeDeclarationSyntax = item.ClassDeclarationSyntax.Parent as TypeDeclarationSyntax;
    //                    var typeName = new StringBuilder()
    //                        .Append(Def.Key_Partial)
    //                        .Append(Def.Fil_Space)
    //                        .Append(typeDeclarationSyntax.Keyword.ValueText)
    //                        .Append(Def.Fil_Space)
    //                        .Append(typeDeclarationSyntax.Identifier.ToString())
    //                        .Append(typeDeclarationSyntax.TypeParameterList)
    //                        .Append(Def.Fil_Space)
    //                        .Append(typeDeclarationSyntax.ConstraintClauses.ToString());
    //                    item.SetTypeName(typeName.ToString());
    //                    workItem = item;
    //                    return true;
    //                }
    //            }
    //        }

    //        workItem = null;
    //        return false;
    //    }
    //}
}
