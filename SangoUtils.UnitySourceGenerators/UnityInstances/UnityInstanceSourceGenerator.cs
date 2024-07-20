using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using SangoUtils.UnitySourceGenerators.Utils;
using System.Collections.Generic;
using System.Text;

namespace SangoUtils.UnitySourceGenerators.UnityInstances
{
    [Generator]
    internal sealed class UnityInstanceSourceGenerator : ISourceGenerator
    {
        public const string UnityInstanceAttributeName = "UnityInstance";

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(delegate { return new UnityInstanceSyntaxReceiver(); });
        }

        public void Execute(GeneratorExecutionContext context)
        {
            string UnityInstanceAttributeSourceText = Def.Dom_Declaration +
$@"
using System;

namespace {Def.Dom_Generateds}
{{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class {UnityInstanceAttributeName}Attribute : Attribute
    {{       
    }}
}}
";

            var moduleName = context.Compilation.SourceModule.Name;
            if (moduleName.StartsWith("UnityEngine.")) return;
            if (moduleName.StartsWith("UnityEditor.")) return;
            if (moduleName.StartsWith("Unity.")) return;

            var sourceText0 = SourceText.From(UnityInstanceAttributeSourceText, System.Text.Encoding.UTF8);
            context.AddSource(UnityInstanceAttributeName + "Attribute.g.cs", sourceText0);

            var syntaxRecevier = context.SyntaxReceiver as UnityInstanceSyntaxReceiver;
            if (syntaxRecevier.CandidateWorkItems.Count == 0) return;

            var codeWriter = new CodeWriter();
            foreach (var workItems in syntaxRecevier.CandidateWorkItems.Values)
            {
                var workItem = workItems[0];
                var semanticModel = context.Compilation.GetSemanticModel(workItem.ClassDeclarationSyntax.SyntaxTree);
                if (semanticModel.GetDeclaredSymbol(workItem.ClassDeclarationSyntax) is INamedTypeSymbol typeSymbol
                    && typeSymbol != null)
                {
                    string typeName = TypeDeclarationSyntaxHelper.WriteTypeName(semanticModel,
                        workItem.ClassDeclarationSyntax);

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
            string namespaceName, string typeName, List<ClassWorkItem> workItems)
        {
            codeWriter.AppendLine(Def.Dom_Declaration);
            codeWriter.AppendLine();
            codeWriter.AppendLine("using UnityEngine;");
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
            ClassWorkItem workItem)
        {
            var className = workItem.ClassDeclarationSyntax.Identifier.ValueText;

            var sourceText = 
$@"
        private static {className}? _instance;
        public static {className} Instance
        {{
            get
            {{
                if (_instance == null)
                {{
                    _instance = FindFirstObjectByType(typeof({className})) as {className};
                    if (_instance == null)
                    {{
                        GameObject gameObject = new GameObject(""["" + typeof({className}).FullName + ""]"");
                        _instance = gameObject.AddComponent<{className}>();
                        gameObject.hideFlags = HideFlags.HideInHierarchy;
                        DontDestroyOnLoad(gameObject);
                    }}
                }}
                return _instance;
            }}
        }}

        private void OnAwake()
        {{
            if (_instance != null && _instance != this)
            {{
                Destroy(gameObject);
            }}
        }}
";

            codeWriter.AppendLine(sourceText);
        }
    }
}
