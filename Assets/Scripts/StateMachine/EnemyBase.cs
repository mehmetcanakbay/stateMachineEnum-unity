using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_EnemyStateMachineStates {
    Idle,
    Defend,
    Attack
}

public class EnemyBase : MonoBehaviour
{

    bool testFlag = false;
    StateMachine<E_EnemyStateMachineStates> stateMachine;
    // Start is called before the first frame update
    void Start()
    {

        stateMachine = new StateMachine<E_EnemyStateMachineStates>(
            E_EnemyStateMachineStates.Idle,
            new StateDataInjection(
                this.gameObject, 
                new GameObject() // TODO: Replace this
            )
        );

        stateMachine.SetAvailableStates(new List<State>{
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

        stateMachine.StartMachine();

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
        stateMachine.TickMachine();
    }
}
