using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SangoUtils.UnitySourceGenerators.Utils;
using System.Collections.Generic;
using System.Text;

namespace SangoUtils.UnitySourceGenerators.FieldChangedNotifys
{
    [Generator]
    internal sealed class FieldChangedNotifySourceGenerator : ISourceGenerator
    {
        public const string FieldChangedNotifyAttributeName = "FieldChangedNotify";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(delegate { return new FieldChangedNotifySyntaxReceiver(); });
        }

        public void Execute(GeneratorExecutionContext context)
        {
            string FieldChangedNotifyAttributeSourceText = Def.Dom_Declaration +
$@"
using System;

namespace {Def.Dom_Generateds}
{{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class {FieldChangedNotifyAttributeName}Attribute : Attribute
    {{       
    }}
}}
";

            var moduleName = context.Compilation.SourceModule.Name;
            if (moduleName.StartsWith("UnityEngine.")) return;
            if (moduleName.StartsWith("UnityEditor.")) return;
            if (moduleName.StartsWith("Unity.")) return;

            var sourceText0 = SourceText.From(FieldChangedNotifyAttributeSourceText, System.Text.Encoding.UTF8);
            context.AddSource(FieldChangedNotifyAttributeName  + "Attribute.g.cs", sourceText0);

            var syntaxRecevier = context.SyntaxReceiver as FieldChangedNotifySyntaxReceiver;
            if (syntaxRecevier.CandidateWorkItems.Count == 0) return;

            var codeWriter = new CodeWriter();
            foreach (var workItems in syntaxRecevier.CandidateWorkItems.Values)
            {
                var workItem = workItems[0];
                var semanticModel = context.Compilation.GetSemanticModel(workItem.PropertyDeclarationSyntax.SyntaxTree);
                if (semanticModel.GetDeclaredSymbol(workItem.PropertyDeclarationSyntax.Parent) is INamedTypeSymbol typeSymbol
                    && typeSymbol != null)
                {
                    string typeName = TypeDeclarationSyntaxHelper.WriteTypeName(semanticModel,
                        workItem.PropertyDeclarationSyntax.Parent as TypeDeclarationSyntax);

                    string namespaceName = NamespaceHelper.GetNamespacePath(typeSymbol.ContainingNamespace);

                    var sourceTextStr = AppendClassBody(codeWriter, semanticModel,
                        namespaceName, typeName, workItems);
                    var sourceText1 = SourceText.From(sourceTextStr, Encoding.UTF8);
                    context.AddSource(typeSymbol.Name + ".g.cs", sourceText1);
                    codeWriter.Clear();
                }
            }
        }

        private static string AppendClassBody(in CodeWriter codeWriter, in SemanticModel semanticModel,
            string namespaceName, string typeName, List<PropertyWorkItem> workItems)
        {
            codeWriter.AppendLine(Def.Dom_Declaration);
            codeWriter.AppendLine();
            codeWriter.AppendLine("using UnityEngine;");
            codeWriter.AppendLine("using UnityEngine.Events;");
            codeWriter.AppendLine();
            if (!string.IsNullOrEmpty(namespaceName))
            {
                codeWriter.AppendLine("namespace " + namespaceName);
                codeWriter.BeginBlock();
            }

            codeWriter.AppendLine(typeName);
            codeWriter.BeginBlock();

            foreach (var workItem in workItems)
            {
                AppendPrivateField(codeWriter, semanticModel,
                        workItem);
            }

            codeWriter.EndBlock();
            if (!string.IsNullOrEmpty(namespaceName))
            {
                codeWriter.EndBlock();
            }

            return codeWriter.ToString();
        }

        private static void AppendPrivateField(in CodeWriter codeWriter, in SemanticModel semanticModel,
            PropertyWorkItem workItem)
        {
            var fieldType = semanticModel.GetTypeInfo(workItem.PropertyDeclarationSyntax.Type).Type.ToDisplayString();
            var fieldName = workItem.PropertyDeclarationSyntax.Identifier.ValueText;

            var sourceText = 
$@"
        private {fieldType} _{fieldName};
        private UnityEvent On{fieldName}Changed = new UnityEvent();
";

            codeWriter.AppendLine(sourceText);
        }
    }
}
