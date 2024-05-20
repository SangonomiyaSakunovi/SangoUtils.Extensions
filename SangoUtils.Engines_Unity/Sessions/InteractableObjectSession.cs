using SangoUtils.Bases_Unity;
using SangoUtils.Bases_Unity.InteractableObjects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SangoUtils.Engines_Unity
{
    public class InteractableObjectSession : MonoBehaviour
    {
#pragma warning disable CS8618
        public static InteractableObjectSession Instance { get; private set; }
#pragma warning restore CS8618
        public Action<string> LogErrorFunc { get; set; } = Debug.LogError;

        private readonly ConcurrentDictionary<int, PressedInteractableObjectPack> _pressedObjectsDict = new ConcurrentDictionary<int, PressedInteractableObjectPack>();
        private readonly ConcurrentDictionary<int, GrabInteractableObjectPack> _grabObjectsDict = new ConcurrentDictionary<int, GrabInteractableObjectPack>();
        
        public ICollection<PressedInteractableObjectPack> GetPressedInteractableObjectPacks() => _pressedObjectsDict.Values;

        /// <summary>
        /// Warning: You must call this API before using any other APIs.
        /// </summary>
        public void Initialize()
        {
            if (Instance != null)
            {
                LogErrorFunc("TrackableComponentSession is already initialized.");
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
            AddEntityAsInteractableObjects(components);
        }

        public void AddEntityAsInteractableObjects(Component[] components)
        {
            foreach (var component in components)
            {
                if (component != null)
                {
                    Type type = component.GetType();
                    AddComponentsAsPressedInteractableObject(component, type);
                    AddComponentAsGrabInteractableObject(component, type);
                }
            }
        }

        public void AddComponentsAsPressedInteractableObject(Component component, Type type)
        {
            if (Attribute.IsDefined(type, typeof(PressedInteractableObjectAttribute)) && typeof(IPressedInteractableObject).IsAssignableFrom(type))
            {
                GameObject entityObject = component.gameObject;
                int entityID = entityObject.GetInstanceID();
                PressedInteractableObjectAttribute attribute = (PressedInteractableObjectAttribute)Attribute.GetCustomAttribute(type, typeof(PressedInteractableObjectAttribute));
                int entityGroupID = attribute.InteractableObjectGroupID;

                if (!_pressedObjectsDict.ContainsKey(entityID))
                {
                    PressedInteractableObjectPack pack = new PressedInteractableObjectPack(entityID, entityGroupID, entityObject);
                    IPressedInteractableObject? meta = component as IPressedInteractableObject;
                    if (meta != null)
                    {
                        pack.OnPressed = meta.OnPressed;
                        pack.OnReleased = meta.OnReleased;
                    }
                    _pressedObjectsDict.TryAdd(entityID, pack);
                }
            }
        }

        public void AddComponentAsGrabInteractableObject(Component component, Type type)
        {
            if (Attribute.IsDefined(type, typeof(GrabInteractableObjectAttribute)) && typeof(IGrabInteractableObject).IsAssignableFrom(type))
            {
                GameObject entityObject = component.gameObject;
                int entityID = entityObject.GetInstanceID();
                GrabInteractableObjectAttribute attribute = (GrabInteractableObjectAttribute)Attribute.GetCustomAttribute(type, typeof(GrabInteractableObjectAttribute));
                int entityGroupID = attribute.InteractableObjectGroupID;

                if (!_grabObjectsDict.ContainsKey(entityID))
                {
                    GrabInteractableObjectPack pack = new GrabInteractableObjectPack(entityID, entityGroupID, entityObject);
                    IGrabInteractableObject? meta = component as IGrabInteractableObject;
                    if (meta != null)
                    {
                        pack.OnPressed = meta.OnPressed;
                        pack.OnReleased = meta.OnReleased;
                    }
                    _grabObjectsDict.TryAdd(entityID, pack);
                }
            }
        }
    }
}
