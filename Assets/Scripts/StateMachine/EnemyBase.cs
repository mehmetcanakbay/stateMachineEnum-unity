using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAKStateMachine;

public enum E_EnemyStateMachineStates {
    Idle,
    Ultimate,
    Defend,
    Attack,
    Any
}

public class EnemyBase : MonoBehaviour
{

    UnityStateMachineRunner<E_EnemyStateMachineStates, SimpleEnemyStateData> stateMachine;
    [SerializeField]
    private int ultimateDamage = -2;
    // Start is called before the first frame update
    void Start()
    {
        var stateData = new SimpleEnemyStateData(
            this, GameManager.Instance.Player.GetComponent<PlayerManager>()
        );
        
        stateMachine = new UnityStateMachineRunner<E_EnemyStateMachineStates, SimpleEnemyStateData>(
            E_EnemyStateMachineStates.Attack,
            stateData
        );

        stateMachine.SetAvailableStates(new List<State<SimpleEnemyStateData>>{
            new EnemyIdleState(),
            new EnemyUltimateState(ultimateDamage),
            new EnemyDefendState(),
            new EnemyAttackState(),
            new EnemyAnyState()
        });
        
        
        stateMachine.AddTransition(
            E_EnemyStateMachineStates.Attack,
            E_EnemyStateMachineStates.Ultimate, 
            CanDoUltimate
        );
    }

    public bool CanDoUltimate() {
        if (GameManager.Instance != null) {
            if (GameManager.Instance.ReturnCurrentEnemyMana() >= 7) {
                // Debug.Log("Doing ultimate attack!!");
                //Attack player here
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

    // Update is called once per frame
    void Update()
    {
    }
}
