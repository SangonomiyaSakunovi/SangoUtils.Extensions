using SangoUtils.Bases_Unity;
using SangoUtils.Bases_Unity.RecognizableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SangoUtils.Engines_Unity
{
    public class RecognizableObjectSession : MonoBehaviour
    {
#pragma warning disable CS8618
        public static RecognizableObjectSession Instance { get; private set; }
#pragma warning restore CS8618
        public Action<string> LogErrorFunc { get; set; } = Debug.LogError;

        private readonly Dictionary<int, MarkerRecognizableObjectPack> _recognizableObjectsDict = new Dictionary<int, MarkerRecognizableObjectPack>();

        public ICollection<MarkerRecognizableObjectPack> GetMarkerRecognizableObjectPacks() => _recognizableObjectsDict.Values;

        public void Initialize()
        {
            if (Instance != null)
            {
                LogErrorFunc("RecognizableObjectSession is already initialized.");
                return;
            }

            Instance = this;
        }

        public void AddEntity()
        {
            Scene scene = SceneManager.GetActiveScene();
            GameObject[] rootObjects = scene.GetRootGameObjects();
            AddEntity(rootObjects);
        }

        public void AddEntity(GameObject[] rootObjects)
        {
            for (int i = 0; i < rootObjects.Length; i++)
            {
                AddEntity(rootObjects[i]);
            }
        }

        public void AddEntity(GameObject rootObject)
        {
            Component[] components = rootObject.GetComponentsInChildren<Component>(true);
            AddEntityAsRecognizableObjects(components);
        }

        public void AddEntityAsRecognizableObjects(Component[] components)
        {
            foreach (var component in components)
            {
                if (component != null)
                {
                    Type type = component.GetType();
                    AddComponentsAsMarkerRecognizableObject(component, type);
                }
            }
        }

        public void AddComponentsAsMarkerRecognizableObject(Component component, Type type)
        {
            if (Attribute.IsDefined(type, typeof(MarkerRecognizableObjectAttribute)) && typeof(IMarkerRecognizableObject).IsAssignableFrom(type))
            {
                GameObject entityObject = component.gameObject;
                int entityID = entityObject.GetInstanceID();
                MarkerRecognizableObjectAttribute attribute = (MarkerRecognizableObjectAttribute)Attribute.GetCustomAttribute(type, typeof(MarkerRecognizableObjectAttribute));
                int entityGroupID = attribute.RecognizableObjectGroupID;

                if (!_recognizableObjectsDict.ContainsKey(entityID))
                {
                    MarkerRecognizableObjectPack pack = new MarkerRecognizableObjectPack(entityID, entityGroupID, entityObject);
                    IMarkerRecognizableObject? meta = component as IMarkerRecognizableObject;
                    if (meta != null)
                    {
                        pack.OnRecognized = meta.OnRecognized;
                        pack.OnLost = meta.OnLost;
                    }
                    _recognizableObjectsDict.Add(entityID, pack);
                }
            }
        }
    }
}
