using UnityEngine;
using UnityEngine.Events;
using MAKStateMachine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace MAKStateMachine {
    public class UnityStateMachineInitializer : MonoBehaviour {
        
        public static UnityStateMachineInitializer Instance;
        [HideInInspector]
        public UnityEvent startEvent;
        [HideInInspector]
        public UnityEvent updateEvent;

        private void Awake() {
            Instance = this;
        }
        

        private List<ITickable> tickableList;
        private List<IInitializable> initializableList;
        
        private void Start() {
            startEvent.Invoke();
            //Reflection test
            /*
            tickableList = new List<ITickable>();
            List<Type> tickableTypes = GetClassesImplementingInterface<ITickable>();
            
            foreach(Type tickable in tickableTypes) {
                if (tickable.IsGenericType) { //this probably needs more queries 
                    Type testtypes = tickable.GetGenericArguments()[0];
                    Debug.Log(testtypes);

                    var genericType = tickable.MakeGenericType(new Type[] {typeof(System.Enum), typeof(IBaseStateData)});
                    tickableList.Add((ITickable)Activator.CreateInstance(genericType));

                } else {
                    tickableList.Add((ITickable)Activator.CreateInstance(tickable));
                }
            }

            //////////////

            // initializableList = new List<IInitializable>();
            // var initializableTypes = GetClassesImplementingInterface<IInitializable>();
            
            // foreach(var initializable in initializableTypes) {
            //     IInitializable doesImplement = (IInitializable)Activator.CreateInstance(initializable);
            //     doesImplement.Initialize();
            //     initializableList.Add(doesImplement);
                
            // }  
            */
        }

        private void Update() {
            updateEvent.Invoke();
            //Reflection test
            /*
            foreach (var tickable in tickableList) {
                tickable.Tick();
            }
            */
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