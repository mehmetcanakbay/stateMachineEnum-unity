using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAKStateMachine;

public enum E_EnemyStateMachineStates {
    Idle,
    Defend,
    Attack
}

public class EnemyBase : MonoBehaviour
{

    bool testFlag = false;
    UnityStateMachineRunner<E_EnemyStateMachineStates, MyGameStateData> stateMachine;
    // Start is called before the first frame update
    void Start()
    {
        var stateData = new MyGameStateData(
            this.gameObject, new GameObject(), "hello"
        );
        
        stateMachine = new UnityStateMachineRunner<E_EnemyStateMachineStates, MyGameStateData>(
            E_EnemyStateMachineStates.Idle,
            stateData
        );

        stateMachine.SetAvailableStates(new List<State<MyGameStateData>>{
            new EnemyIdleState(),
            new EnemyDefendState(),
            new EnemyAttackState()
        });

        stateMachine.AddTransition(
            E_EnemyStateMachineStates.Idle,
            E_EnemyStateMachineStates.Defend, 
            ()=>testFlag
        );

        stateMachine.AddTransition(
            E_EnemyStateMachineStates.Defend,
            E_EnemyStateMachineStates.Attack, 
            ()=>true
        );

        stateMachine.AddTransition(
            E_EnemyStateMachineStates.Attack,
            E_EnemyStateMachineStates.Idle, 
            ()=>!testFlag
        );

        StartCoroutine(testingDelay());
    }

    IEnumerator testingDelay() {
        yield return new WaitForSeconds(5.0f);
        testFlag = true;
        yield return new WaitForSeconds(2.0f);
        testFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
