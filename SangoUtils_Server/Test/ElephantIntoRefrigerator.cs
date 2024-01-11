using System.Collections.Concurrent;

namespace SangoUtils_Server.Test
{
    public abstract class SingletonExample<T> where T : class, new() //Singleton
    {
        public static T Instance
        {
            get
            {
                Instance ??= new T();
                return Instance;
            }
            private set { Instance = value; }
        }
    }

    public abstract class BaseAnimalExamlpe //Structor Scripts
    {
        public abstract void OnMessage(string message);
        public void OnInit<T>(T t) where T : BaseAnimalExamlpe
        {
            BoxServiceExample.Instance.AddAnimal(t);
        }
    }
    public class BoxServiceExample : SingletonExample<BoxServiceExample> //Structor Scripts
    {
        private ConcurrentDictionary<int, BaseAnimalExamlpe> _animalDict = new();

        public bool AddAnimal(BaseAnimalExamlpe animal)
        {
            return _animalDict.TryAdd(animal.GetHashCode(), animal);
        }
        public T GetAnimal<T>() where T : BaseAnimalExamlpe, new()
        {
            if (_animalDict.TryGetValue(typeof(T).GetHashCode(), out BaseAnimalExamlpe? animal))
            {
                return (T)animal;
            }
            else
            {
                T t = new T();
                t.OnInit<T>(t);
                return t;
            }
        }
        public void OnMessage<T>(string message) where T : BaseAnimalExamlpe
        {
            int key = typeof(T).GetHashCode();
            if (_animalDict.TryGetValue(typeof(T).GetHashCode(), out BaseAnimalExamlpe? animal))
            {
                animal.OnMessage(message);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
