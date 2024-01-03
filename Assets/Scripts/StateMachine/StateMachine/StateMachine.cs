using System;
using System.Collections.Generic;

namespace MAKStateMachine {
    /// <summary>
    /// Struct to send information from the main component to the state easily 
    /// This is transition info, one can create this struct, then push this info to the transition list
    /// And then this transition starts to get checked, if req is true go into the new state etc...
    /// </summary>
    public struct StateTransitionInfo {
        public int from;
        public int to;
        public Func<bool> req;

        public StateTransitionInfo(int fromref, int toref, Func<bool> reqref) {
            from = fromref;
            to = toref;
            req = reqref;
        }
    }

    public class StateMachine<T, F> where T : System.Enum 
                                    where F : IBaseStateData
    {  
        T currentState;
        F stateData;
        Func<T, int> enumConverter;
        List<State<F>> availableStates;
        List<StateTransitionInfo> transitions;

        //compiler stuff
        private static int Identity(int x) {
            return x;
        }

        /// <summary>
        /// Get the index of the enum. Used for state function calls
        /// </summary>
        /// <param name="paramEnum">Returns the index of the given enum</param>
        /// <returns>return the index of the enum</returns>
        private int ReturnIndexOfEnum(T paramEnum) {
            return enumConverter(paramEnum);
        } 

        /// <summary>
        /// Initialize all the states, in the same hierarchy as the enum.
        /// Example: 
        /// stateMachine.SetAvailableStates(new List<State>{
        ///    new EnemyIdleState(),
        ///    new EnemyDefendState(),
        ///    new EnemyAttackState()
        /// });
        /// </summary>
        /// <param name="overridenStateList"></param>
        public void SetAvailableStates(List<State<F>> overridenStateList) {
            availableStates = overridenStateList;
        }

        /// <summary>
        /// Add a transition to this state machine object.
        /// Example: AddTransition(Enum.Idle, Enum.Attack, () => shouldAttack);
        /// </summary>
        /// <param name="from">From which state</param>
        /// <param name="to">To what state</param>
        /// <param name="requirement">Function reference, it must return boolean. If true, it will continue the transition.</param>
        public void AddTransition(T from, T to, Func<bool> requirement) {
            transitions.Add(new StateTransitionInfo(
                ReturnIndexOfEnum(from), ReturnIndexOfEnum(to), requirement
            ));
        }

        /// <summary>
        /// Initialize a state machine. 
        /// After initializing, do not forget these two important steps
        /// Do not forget to initialize the states using SetAvailableStates, hierarchy should be the same as the enum.
        /// Do not forget to call StartMachine and TickMachine in Start and Update functions respectively.
        /// </summary>
        /// <param name="starterState">state to start with</param>
        /// <param name="sharedStateData">State data class to be passed around in states</param>
        public StateMachine(T starterState, F sharedStateData) {
            currentState = starterState;
            stateData = sharedStateData;
            transitions = new List<StateTransitionInfo>();

            //to add int of enums
            //credits to @thefuntastic's state machine repo
            Func<int, int> identity = Identity;
            enumConverter = Delegate.CreateDelegate(typeof(Func<T, int>), identity.Method) as Func<T, int>;
        }
        
        /// <summary>
        /// Starts the initialization for available states.
        /// </summary>
        public void StartMachine() {
            foreach (State<F> state in availableStates) {
                state.Initialize(stateData);
            }
        }

        /// <summary>
        /// Start ticking the machine.
        /// </summary>
        public void TickMachine() {
            foreach (StateTransitionInfo inform in transitions) {
                //if the transition isnt from this state, then no point in checking
                if (inform.from != ReturnIndexOfEnum(currentState)) {
                    continue;
                }

                //if the transition is from this state, then check the requirement
                if (inform.req()) {
                    SwitchStates((T)Enum.ToObject(typeof(T), inform.to));
                }
            }

            //tick the current state
            int currentIndex = enumConverter(currentState);
            availableStates[currentIndex].Tick();
        }

        /// <summary>
        /// Switch the state to another state, and call the transition enter and exit functions.
        /// </summary>
        /// <param name="newState">New state to switch state</param>
        public void SwitchStates(T newState) {
            availableStates[enumConverter(currentState)].OnTransitionExit();
            currentState = newState;
            availableStates[enumConverter(currentState)].OnTransitionEnter();
        }

        public T ReturnCurrentState() {
            return currentState;
        }

        /// <summary>
        /// CAUTION!! 
        /// This re-intializes all states.
        /// </summary>
        /// <param name="newData">New state data.</param>
        public void RenewStateData(F newData) {
            stateData = newData;
            StartMachine();
        }

    }

}
