using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAKStateMachine;

public class EnemyUltimateState : State<SimpleEnemyStateData>
{
    int amountOfDamage = 0;
    public EnemyUltimateState(int amountOfDamageToDo) {
        amountOfDamage = amountOfDamageToDo;
    }

    public override void OnInitialize()
    {
    }

    public override void Tick()
    {
    }

    public override void OnTransitionEnter()
    {
        stateData.playerManager.AddToPlayerHealth(amountOfDamage);
        GameManager.Instance.AddToEnemyMana(-7);
    }

    public override void OnTransitionExit()
    {
    }
}
