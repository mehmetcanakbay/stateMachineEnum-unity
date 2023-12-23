using UnityEngine;
using UnityEngine.Events;
using MAKStateMachine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace MAKStateMachine {
    public class UnityStateMachineInitializer : MonoBehaviour {
        /*
        public static UnityStateMachineInitializer Instance;
        [HideInInspector]
        public UnityEvent startEvent;
        [HideInInspector]
        public UnityEvent updateEvent;

        private void Awake() {
            Instance = this;
        }
        */

        private List<ITickable> tickableList;
        private List<IInitializable> initializableList;
        
        private void Start() {
            // startEvent.Invoke();
            tickableList = new List<ITickable>();
            var tickableTypes = GetClassesImplementingInterface<ITickable>();
            
            foreach(var tickable in tickableTypes) {
                tickableList.Add((ITickable)Activator.CreateInstance(tickable));
            }

            initializableList = new List<IInitializable>();
            var initializableTypes = GetClassesImplementingInterface<IInitializable>();
            
            foreach(var tickable in initializableTypes) {
                IInitializable doesImplement = (IInitializable)Activator.CreateInstance(tickable);
                doesImplement.Initialize();
                initializableList.Add(doesImplement);
                
            }  
        }

        private void Update() {
            // updateEvent.Invoke();
            foreach (var tickable in tickableList) {
                tickable.Tick();
            }
        }

        private List<Type> GetClassesImplementingInterface<T>() {
            Type interfaceType = typeof(T);
            List<Type> implementingTypes = new List<Type>();

            Type[] types = Assembly.GetExecutingAssembly().GetTypes();


            foreach (Type type in types)
            {
                if (interfaceType.IsAssignableFrom(type) && type != interfaceType)
                {
                    implementingTypes.Add(type);
                }
            }

            return implementingTypes;
        }
    }
}