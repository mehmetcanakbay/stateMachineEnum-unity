using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAKStateMachine;

public enum E_GameStates {
    PlayerTurn,
    EnemyTurn,
    AllocatingPhase
}
public class GameStateManager : MonoBehaviour
{
    E_GameStates currState;
    UnityStateMachineRunner<E_GameStates, GameStateData> stateMachine;

    public delegate void OnStateChangeDelegate(E_GameStates state);
    public event OnStateChangeDelegate onStateChange;
    public int enemyAttackNeedMana = 2;

    private void Start() {
        stateMachine = new UnityStateMachineRunner<E_GameStates, GameStateData>(
            E_GameStates.PlayerTurn,
            new GameStateData(
                new GameObject(),
                new GameObject(),
                stateMachine, //sends null,
                this
            )
        );


        stateMachine.SetAvailableStates(
            new List<State<GameStateData>>{
                new StatePlayerTurn(),
                new StateEnemyTurn(enemyAttackNeedMana),
                new StateAllocatingPhase()
            }
        );

        //happens all because of statemachine ref. might be better to refactor the code so user 
        //initializes the state machine like they do available states
        stateMachine.RenewStateData(
            new GameStateData(
                GameManager.Instance.Player,
                GameManager.Instance.Enemy,
                stateMachine,
                this
            )
        );


        //Update the UI text by hand once
        //okay so... this one creates race condition with the UIManager's start
        //this throws null if you dont add the ? there and it sometimes works, so yeah, race condition
        //i could just use a coroutine here... for it to work and I'm actually going to do that
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart() {
        //time based is probably a bad idea
        yield return new WaitForFixedUpdate();
        onStateChange?.Invoke(E_GameStates.PlayerTurn);
    }


    public void ChangeState(E_GameStates stateChange) {
        stateMachine.TransitionToThisStateNow(stateChange);
        onStateChange?.Invoke(stateChange);
        Debug.Log("onstate change");
        Debug.Log(stateChange);
    }

    public void ConfirmEndPlayerTurn() {
        ChangeState(E_GameStates.EnemyTurn);
    }

    public void ConfirmEndEnemyTurn() {
        ChangeState(E_GameStates.AllocatingPhase);
    }

//     public void ConfirmEndAllocatingTurn() {
//         ChangeState(E_GameStates.PlayerTurn);
//     }
}
