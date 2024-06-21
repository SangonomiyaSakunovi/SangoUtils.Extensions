using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Reflection;

namespace SangoServers.Bases_ASPNet.Services
{
    public class WebViewService
    {
        public static WebViewService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Activator.CreateInstance<WebViewService>();
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        private static WebViewService? _instance;

        private readonly ConcurrentDictionary<int, IWebView> _webViewDict = new();

        public void AddWebView()
        {
            TryAddWebView(Assembly.GetExecutingAssembly());

            var assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            foreach (var assemblyName in assemblyNames)
            {
                if (assemblyName.Name!.StartsWith("SangoUtils"))
                {
                    var assembly = Assembly.Load(assemblyName);
                    if (assembly == null) { continue; }

                    TryAddWebView(assembly);
                }
            }
        }

        public void AddWebView(List<Assembly> assemblies)
        {
            for (int i = 0; i < assemblies.Count; i++)
            {
                TryAddWebView(assemblies[i]);
            }
        }

        private void TryAddWebView(Assembly assembly)
        {
            var list = assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract && !x.IsGenericType).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                Type type = list[i];
                if (type != null && typeof(IWebView).IsAssignableFrom(type))
                {
                    int hashCode = type.GetHashCode();
                    if (_webViewDict.ContainsKey(hashCode)) { return; }

                    IWebView? instance = Activator.CreateInstance(type) as IWebView;

                    if (!ApplicationService.IsInitialized) { return; }

                    if (instance != null)
                    {
                        instance.MapView(ApplicationService.App!);
                        bool res = _webViewDict.TryAdd(hashCode, instance);
                        if (!res)
                        {
                            ApplicationService.App!.Logger.LogError($"Cannot Add EndPoint {type.FullName} to EndPointDict.");
                        }
                    }
                }
            }
        }
    }
}
