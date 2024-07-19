using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SangoUtils.UnitySourceGenerators.Utils;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SangoUtils.UnitySourceGenerators.FieldChangedNotifys
{
    [Generator]
    internal sealed class FieldChangedNotifySourceGenerator : ISourceGenerator
    {
        public const string FieldChangedNotifyAttributeName = "FieldChangedNotify";

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif
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
            if (moduleName.StartsWith(Def.Dom_UnityEngine + Def.Sym_Dot)) return;
            if (moduleName.StartsWith(Def.Dom_UnityEditor + Def.Sym_Dot)) return;
            if (moduleName.StartsWith(Def.Dom_Unity + Def.Sym_Dot)) return;

            var sourceText0 = SourceText.From(FieldChangedNotifyAttributeSourceText, System.Text.Encoding.UTF8);
            context.AddSource(FieldChangedNotifyAttributeName + Def.Key_Attribute + Def.Ext_gcs, sourceText0);

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
                    string namespaceName = typeSymbol.ContainingNamespace.IsGlobalNamespace ?
                        string.Empty : typeSymbol.ContainingNamespace.Name;
                    var sourceTextStr = AppendClassBody(codeWriter, semanticModel,
                        namespaceName, typeName, workItems);
                    var sourceText1 = SourceText.From(sourceTextStr, Encoding.UTF8);
                    context.AddSource(typeSymbol.Name + Def.Ext_gcs, sourceText1);
                    codeWriter.Clear();
                }
            }
        }

        private static string AppendClassBody(in CodeWriter codeWriter, in SemanticModel semanticModel,
            string namespaceName, string typeName, List<PropertyWorkItem> workItems)
        {
            codeWriter.AppendLine(Def.Dom_Declaration);
            codeWriter.AppendLine();
            codeWriter.AppendLine(Def.Key_Using + Def.Fil_Space + Def.Dom_UnityEngine + Def.Sym_Semicolon);
            codeWriter.AppendLine(Def.Key_Using + Def.Fil_Space + Def.Dom_UnityEvents + Def.Sym_Semicolon);
            codeWriter.AppendLine();
            if (!string.IsNullOrEmpty(namespaceName))
            {
                codeWriter.AppendLine(Def.Key_Namespace + Def.Fil_Space + namespaceName);
                codeWriter.BeginBlock();
            }

            codeWriter.AppendLine(typeName);
            codeWriter.BeginBlock();

            foreach (var workItem in workItems)
            {
                AppendPrivateField(codeWriter, semanticModel,
                        workItem);
                codeWriter.AppendLine();
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
            var stringBuilder = new StringBuilder();
            var fieldType = semanticModel.GetTypeInfo(workItem.PropertyDeclarationSyntax.Type).Type.ToDisplayString();
            var fieldName = workItem.PropertyDeclarationSyntax.Identifier.ValueText;
            
            var privateFieldName = Def.Sym_Underline + fieldName;
            stringBuilder
                .Append(Def.Fil_Tab)
                .Append(Def.Key_Private)
                .Append(Def.Fil_Space)
                .Append(fieldType)
                .Append(Def.Fil_Space)
                .Append(privateFieldName)
                .Append(Def.Sym_Semicolon);
            codeWriter.AppendLine(stringBuilder.ToString());
            codeWriter.AppendLine();
            stringBuilder.Clear();

            var privateEventName = "On" + fieldName + "Changed";
            stringBuilder
                .Append(Def.Fil_Tab)
                .Append(Def.Key_Private)
                .Append(Def.Fil_Space)
                .Append(Def.Key_UnityEvent)
                .Append(Def.Fil_Space)
                .Append(privateEventName)
                .Append(Def.Fil_Space)
                .Append(Def.Op_Equal)
                .Append(Def.Fil_Space)
                .Append(Def.Key_New)
                .Append(Def.Fil_Space)
                .Append(Def.Key_UnityEvent)
                .Append(Def.Sym_Open)
                .Append(Def.Sym_Close)
                .Append(Def.Sym_Semicolon);
            codeWriter.AppendLine(stringBuilder.ToString());
            codeWriter.AppendLine();
            stringBuilder.Clear();
        }
    }
}
