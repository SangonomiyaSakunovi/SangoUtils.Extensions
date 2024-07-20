using Microsoft.CodeAnalysis;

namespace SangoUtils.UnitySourceGenerators.Utils
{
    internal static class NamespaceHelper
    {
        public static string GetNamespacePath(INamespaceSymbol symbol)
        {
            if (symbol == null || symbol.IsGlobalNamespace)
                return string.Empty;

            string parentPath = GetNamespacePath(symbol.ContainingNamespace);
            string currentName = symbol.Name;

            if (!string.IsNullOrEmpty(parentPath))
                return parentPath + "." + currentName;

            return currentName;
        }
    }
}
