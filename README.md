# State Machine Example Project

This is an example project for a state machine. It is not complete, as it is only an example project.

## Usage Example

First, you need to put UnityStateMachineInitializer into an empty game object in your scene.

Then, you create a data class which inherits from IBaseStateData.
```c#
using MAKStateData;

public class SimpleEnemyStateData : IBaseStateData {
    public EnemyBase enemyBaseRef;
    public PlayerManager playerManager;

    public SimpleEnemyStateData(EnemyBase enemy, 
                            PlayerManager playerManagerComponent) {
        enemyBaseRef = enemy;
        playerManager = playerManagerComponent;
    }
}
```

Of course, you also need some states. Let's create one.

```c#
public class EnemyAttackState : State<SimpleEnemyStateData>
{
    public override void OnInitialize() {
    }

    public override void Tick() {
    }

    public override void OnTransitionEnter() {
        stateData.playerManager.AddToPlayerHealth(-1);
    }

    public override void OnTransitionExit() {
    }
}
```

You also need an enum for the state machine to work. Let's also make an enum.

```c#
public enum E_EnemyStateMachineStates {
    Defend,
    Attack,
    Ultimate
}
```

To initialize and use a state machine, you create an object from UnityStateMachineRunner class.

```c#
//create state data
var stateData = new SimpleEnemyStateData(
    this, GameManager.Instance.Player.GetComponent<PlayerManager>()
);
        
stateMachine = new UnityStateMachineRunner
<E_EnemyStateMachineStates, SimpleEnemyStateData>(
    E_EnemyStateMachineStates.Attack, //starter state
    stateData //statedata
);
```
You then need to send over the states to the state machine runner.
### IMPORTANT! It needs to be in the same order as the enum.
### So if you made an enum with "Attack, Defend", it needs to be fed Attack and Defend respectively!

```c#
stateMachine.SetAvailableStates(
    new List<State<SimpleEnemyStateData>>{
        new EnemyDefendState(),
        new EnemyAttackState(),
        new EnemyUltimateState()
    }
);
```
You can also automatically change to an another state given a flag.

```c#
stateMachine.AddTransition(
    E_EnemyStateMachineStates.Attack,
    E_EnemyStateMachineStates.Ultimate, 
    CanDoUltimate
);
```

Or you could immediately change into another state:
```c#
public void ChangeState(E_EnemyStateMachineStates state) {
    stateMachine.TransitionToThisStateNow(state);
}
```

It is highly recommended that you wrap "TransitionToThisStateNow" with a function. That way, for example, you could fire off events like "On State Change".

That is all. You can call ```ResumeTicking()``` or ```StopTicking()``` to pause the state machine.

Example full usage:

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAKStateMachine;

public enum E_EnemyStateMachineStates {
    Ultimate,
    Defend,
    Attack,
}

public class EnemyBase : MonoBehaviour
{

    UnityStateMachineRunner<E_EnemyStateMachineStates, SimpleEnemyStateData> stateMachine;
    [SerializeField]
    private int ultimateDamage = -2;
    // Start is called before the first frame update
    void Start()
    {
        //create state data
        var stateData = new SimpleEnemyStateData(
            this, GameManager.Instance.Player.GetComponent<PlayerManager>()
        );

        //initialize the state machine        
        stateMachine = new UnityStateMachineRunner<E_EnemyStateMachineStates, SimpleEnemyStateData>(
            E_EnemyStateMachineStates.Attack,
            stateData
        );

        //set all available states here
        stateMachine.SetAvailableStates(new List<State<SimpleEnemyStateData>>{
            new EnemyUltimateState(ultimateDamage),
            new EnemyDefendState(),
            new EnemyAttackState(),
        });
        
        //auto transition to ultimate if its possible
        stateMachine.AddTransition(
            E_EnemyStateMachineStates.Attack,
            E_EnemyStateMachineStates.Ultimate, 
            CanDoUltimate
        );
    }

    public bool CanDoUltimate() {
        if (GameManager.Instance != null) {
            if (GameManager.Instance.ReturnCurrentEnemyMana() >= 7) {
                return true;
            }   
            else {
                return false;
            }
        }
        return false;
    }

    public void ChangeState(E_EnemyStateMachineStates state) {
        stateMachine.TransitionToThisStateNow(state);
    }
}
```