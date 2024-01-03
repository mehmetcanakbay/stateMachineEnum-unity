using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using MAKStateMachine;
using System;

namespace MAKStateMachine {
    public class UnityStateMachineRunner<T, F>: IInitializable, ITickable  where T : System.Enum 
                                                                            where F : IBaseStateData   {
        private StateMachine<T,F> stateMachine;

        private bool shouldTick = true;

        public UnityStateMachineRunner(T startState, F info) {
            stateMachine = new StateMachine<T, F>(
                startState, info
            );
        }

        public void SetAvailableStates(List<State<F>> overridenStateList) {
            stateMachine.SetAvailableStates(overridenStateList);
            BindToEngine();
            Initialize();
        }

        public void AddTransition(T from, T to, Func<bool> requirement) {
            stateMachine.AddTransition(
                from, to, requirement
            );
        }

        public void TransitionToThisStateNow(T to) {
            stateMachine.SwitchStates(to);
        }

        bool BindToEngine() {
            
            if (UnityStateMachineInitializer.Instance != null) {
                //I think start event is not working...
                // UnityStateMachineInitializer.Instance.startEvent.AddListener(CustomStart);
                UnityStateMachineInitializer.Instance.updateEvent.AddListener(Tick);
                return true;
            } else {
                Debug.LogWarning("Initializer does not exist!! Did you forget to add UnityStateMachineInitializer component to your scene?");
                return false;
            }
        }

        public void Tick() {
            if (shouldTick) {
                stateMachine.TickMachine();
                Debug.Log("ticking");
            }
        }

        public void Initialize() {
            stateMachine.StartMachine();
        }

        public void StopTicking() {
            shouldTick = false;
        }

        public void ResumeTicking() {
            shouldTick = true;
        }

        public T ReturnCurrentState() {
            return stateMachine.ReturnCurrentState();
        }

        public void RenewStateData(F data) {
            stateMachine.RenewStateData(data);
        }

    }
}
