using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SangoUtils.Engines_Unity.Utilities
{
    public static class MethodsUtils_Unity
    {
        public static IEnumerable<MethodDesc_BasicParam_1> CollectMethods_BasicParam(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return Enumerable.Empty<MethodDesc_BasicParam_1>();
            }

            List<MethodDesc_BasicParam_1> collectedMethods = new List<MethodDesc_BasicParam_1>();
            MonoBehaviour[] behaviours = gameObject.GetComponents<MonoBehaviour>();

            foreach (var behaviour in behaviours)
            {
                if (behaviour == null)
                {
                    continue;
                }

                Type type = behaviour.GetType();
                while (type != typeof(MonoBehaviour) && type != null)
                {
                    MethodInfo[] methods = type.GetMethods(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                    foreach (MethodInfo method in methods)
                    {
                        string name = method.Name;

                        if (IsMonobehaviorMethod(name))
                        {
                            continue;
                        }
                        if (!Attribute.IsDefined(method, typeof(CollectedMethodAttribute)))
                        {
                            continue;
                        }

                        ParameterInfo[] parameters = method.GetParameters();
                        if (parameters.Length > 1)
                        {
                            continue;
                        }

                        MethodParameterType parameterType = MethodParameterType.None;
                        if (parameters.Length == 1)
                        {
                            Type paramType = parameters[0].ParameterType;

                            if (paramType == typeof(string))
                                parameterType = MethodParameterType.String;
                            else if (paramType == typeof(float))
                                parameterType = MethodParameterType.Float;
                            else if (paramType == typeof(int))
                                parameterType = MethodParameterType.Int;
                            else if (paramType == typeof(UnityEngine.Object) || paramType.IsSubclassOf(typeof(UnityEngine.Object)))
                                parameterType = MethodParameterType.Object;
                            else
                                continue;
                        }

                        MethodDesc_BasicParam_1 supportedMethod = new MethodDesc_BasicParam_1(name, parameterType);

                        // Since AnimationEvents only stores method name, it can't handle functions with multiple overloads.
                        // Only retrieve first found function, but discard overloads.
                        var existingMethodIndex = collectedMethods.FindIndex(m => m.name == name);
                        if (existingMethodIndex != -1)
                        {
                            // The method is only ambiguous if it has a different signature to the one we saw before
                            MethodDesc_BasicParam_1 existingMethod = collectedMethods[existingMethodIndex];
                            existingMethod.isOverload = existingMethod.type != parameterType;
                        }
                        else
                            collectedMethods.Add(supportedMethod);
                    }
                    type = type.BaseType;
                }
            }

            return collectedMethods;
        }

        private static bool IsMonobehaviorMethod(string name) => name switch
        {
            "Main" => true,
            "Awake" => true,
            "OnEnable" => true,
            "Reset" => true,
            "Start" => true,
            "FixedUpdate" => true,
            "Update" => true,
            "LateUpdate" => true,
            "OnDisable" => true,
            "OnDestroy" => true,
            _ => false
        };
    }
}
