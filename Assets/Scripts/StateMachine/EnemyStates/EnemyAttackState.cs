using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAKStateMachine;

public class EnemyAttackState : State<SimpleEnemyStateData>
{
    public override void OnInitialize()
    {
    }

    public override void Tick()
    {
    }

    public override void OnTransitionEnter()
    {
        stateData.playerManager.AddToPlayerHealth(-1);
    }

    public override void OnTransitionExit()
    {
    }
}
