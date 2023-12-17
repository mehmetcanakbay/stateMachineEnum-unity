using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using MAKStateMachine;
using System;

namespace MAKStateMachine {
    public class UnityStateMachineRunner<T, F> where T : struct 
                                                where F : IBaseStateData {
        private StateMachine<T,F> stateMachine;

        private bool shouldTick = true;

        public UnityStateMachineRunner(T startState, F info) {
            stateMachine = new StateMachine<T, F>(
                startState, info
            );
        }

        public void SetAvailableStates(List<State<F>> overridenStateList) {
            BindToEngine();

            stateMachine.SetAvailableStates(overridenStateList);
        }

        public void AddTransition(T from, T to, Func<bool> requirement) {
            stateMachine.AddTransition(
                from, to, requirement
            );
        }

        bool BindToEngine() {
            if (UnityStateMachineInitializer.Instance != null) {
                UnityStateMachineInitializer.Instance.startEvent.AddListener(CustomStart);
                UnityStateMachineInitializer.Instance.updateEvent.AddListener(CustomUpdate);
                return true;
            } else {
                Debug.LogWarning("Initializer does not exist!! Did you forget to add UnityStateMachineInitializer component to your scene?");
                return false;
            }
        }

        void CustomStart() {
            stateMachine.StartMachine();
        }

        void CustomUpdate() {
            if (shouldTick) {
                stateMachine.TickMachine();
            }
        }

        public void StopTicking() {
            shouldTick = false;
        }

        public void ResumeTicking() {
            shouldTick = true;
        }


    }
}
