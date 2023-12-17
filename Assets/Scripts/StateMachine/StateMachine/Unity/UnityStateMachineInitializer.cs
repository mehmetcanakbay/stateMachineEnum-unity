using UnityEngine;
using UnityEngine.Events;
using MAKStateMachine;

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
        
        private void Start() {
            startEvent.Invoke();
        }

        private void Update() {
            updateEvent.Invoke();
        }
    }
}