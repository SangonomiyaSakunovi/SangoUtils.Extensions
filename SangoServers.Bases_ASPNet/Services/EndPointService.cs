using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Reflection;

namespace SangoServers.Bases_ASPNet.Services
{
    public class EndPointService
    {
        public static EndPointService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Activator.CreateInstance<EndPointService>();
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        private static EndPointService? _instance;

        private readonly ConcurrentDictionary<int, IEndPoint> _endPointsDict = new();

        public void AddEndPoint()
        {
            TryAddEndPoint(Assembly.GetExecutingAssembly());

            var assemblyNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            foreach (var assemblyName in assemblyNames)
            {
                Console.WriteLine($"AssemblyName: {assemblyName.Name}");
                if (assemblyName.Name!.StartsWith("SangoUtils"))
                {
                    var assembly = Assembly.Load(assemblyName);
                    if (assembly == null) { continue; }

                    TryAddEndPoint(assembly);
                }
            }
        }

        public void AddEndPoint(List<Assembly> assemblies)
        {
            for (int i = 0; i < assemblies.Count; i++)
            {
                TryAddEndPoint(assemblies[i]);
            }
        }

        private void TryAddEndPoint(Assembly assembly)
        {
            var list = assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract && !x.IsGenericType).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                Type type = list[i];
                if (type != null && typeof(IEndPoint).IsAssignableFrom(type))
                {
                    int hashCode = type.GetHashCode();
                    if (_endPointsDict.ContainsKey(hashCode)) { return; }

                    IEndPoint? instance = Activator.CreateInstance(type) as IEndPoint;

                    if (!ApplicationService.IsInitialized) { return; }

                    if (instance != null)
                    {
                        instance.MapPoint(ApplicationService.App!);
                        bool res = _endPointsDict.TryAdd(hashCode, instance);
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
