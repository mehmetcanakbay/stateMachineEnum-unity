using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

//well this seems like a bad idea, it probably holds a lot more data than it needs
//TODO: convert from gameobject to component
public struct StateDataInjection {
    public GameObject owner;
    public GameObject player;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="ownerObj">State owner</param>
    /// <param name="playerObj">Player reference</param>
    public StateDataInjection(GameObject ownerObj, GameObject playerObj) {
        owner = ownerObj;
        player = playerObj;
    }
}

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

public class StateMachine<T> where T : struct
{  
    T currentState;
    StateDataInjection stateDataInjects;
    Func<T, int> enumConverter;
    List<State> availableStates;
    List<StateTransitionInfo> transitions;

    //compiler stuff
    private static int Identity(int x) {
        return x;
    }

    /// <summary>
    /// Get the index of the enum. Used for state function calls
    /// </summary>
    /// <param name="paramEnum">Returns the index of the given enum</param>
    /// <returns></returns>
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
    public void SetAvailableStates(List<State> overridenStateList) {
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
    /// <param name="starterState"></param>
    public StateMachine(T starterState, StateDataInjection data) {
        currentState = starterState;
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
        foreach (State state in availableStates) {
            state.OnInitialize(stateDataInjects);
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
        availableStates[currentIndex].Tick(stateDataInjects);
    }

    /// <summary>
    /// Switch the state to another state, and call the transition enter and exit functions.
    /// This is private and user should not be able to touch this.
    /// </summary>
    /// <param name="newState">New state to switch state</param>
    private void SwitchStates(T newState) {
        availableStates[enumConverter(currentState)].OnTransitionExit(stateDataInjects);
        currentState = newState;
        availableStates[enumConverter(currentState)].OnTransitionEnter(stateDataInjects);
    }

}
