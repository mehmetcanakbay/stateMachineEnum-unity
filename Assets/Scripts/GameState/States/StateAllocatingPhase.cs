using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAKStateMachine;

public class StateAllocatingPhase : State<GameStateData>
{
    float duration = 2.0f;

    //do animations etc. then after this, set to true and start to exit from state
    public override void OnInitialize()
    {
    }

    public override void Tick()
    {
        if (duration > 0.0f) {
            duration -= Time.deltaTime;
        } else {
            stateData.gameStateManager.ChangeState(E_GameStates.PlayerTurn);
        }
    }

    public override void OnTransitionEnter()
    {
        //accepting it using instance all the time doesnt look good, lets just cache it
        GameManager instanceRef;
        if (GameManager.Instance == null) {
            Debug.LogWarning("Game Manager instance is null!! This state cannot do anything.");
            return;
        } else {
            instanceRef = GameManager.Instance;
        }

        Random.InitState(System.DateTime.Now.Millisecond);

        //not sure, but I think copying the arrays would result in slower code than accesing it like this? 
        //+1 because it needs to be inclusive
        int randomAmountToAddToPlayer = Random.Range(
            instanceRef.randomAmountClosedPlayer[0], 
            instanceRef.randomAmountClosedPlayer[1]+1
        );


        //Todo: Do animation

        System.Random randGen = new System.Random();
        int randomAmountToAddToEnemy = randGen.Next(
            instanceRef.randomAmountClosedEnemy[0], 
            instanceRef.randomAmountClosedEnemy[1]+1
        );
        //TODO: Do anim


        GameManager.Instance.AddToEnemyMana(randomAmountToAddToEnemy);
        GameManager.Instance.AddToPlayerMana(randomAmountToAddToPlayer);

        //after all of it, exit transition
        // stateData.gameStateManager.ChangeState(E_GameStates.PlayerTurn);
        //stateData.stateMachine.TransitionToThisStateNow(E_GameStates.PlayerTurn);
    }

    public override void OnTransitionExit()
    {
        duration = 2.0f;

    }
}
