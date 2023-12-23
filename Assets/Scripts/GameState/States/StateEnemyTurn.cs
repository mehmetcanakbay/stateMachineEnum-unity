using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAKStateMachine;

public class StateEnemyTurn : State<GameStateData>
{
    EnemyBase enemyBaseComponent;
    int enemyAttackNeedMana;
    float duration = 2.0f;

    public StateEnemyTurn(int enemyAttackNeedManaParam) {
        enemyAttackNeedMana = enemyAttackNeedManaParam;
    }

    public override void OnInitialize()
    {
        enemyBaseComponent = stateData.enemy.GetComponent<EnemyBase>();
    }

    public override void Tick()
    {
        if (duration > 0.0f) {
            duration -= Time.deltaTime;
        } else {
            stateData.gameStateManager.ChangeState(E_GameStates.AllocatingPhase);
        }
    }

    public override void OnTransitionEnter()
    {
        //either attack or defend. ultimate happens automatically
        int twoChoices = Random.Range(0, 2);
        Debug.Log(twoChoices);

        //if i had more choices here, I would've used a switch.
        if (twoChoices == 0 && GameManager.Instance != null && GameManager.Instance.ReturnCurrentEnemyMana() >= enemyAttackNeedMana) {
            enemyBaseComponent.ChangeState(E_EnemyStateMachineStates.Attack);
            GameManager.Instance.AddToEnemyMana(-enemyAttackNeedMana);
            GameManager.Instance.PushNotif("Enemy attacks!");
            Debug.Log("Enemy attacks.");
        }

        if (twoChoices == 1) {
            enemyBaseComponent.ChangeState(E_EnemyStateMachineStates.Defend);
            GameManager.Instance.PushNotif("Enemy defends.");
            Debug.Log("Enemy defends.");
        }

    }

    public override void OnTransitionExit()
    {
        duration = 2.0f;
    }
}
