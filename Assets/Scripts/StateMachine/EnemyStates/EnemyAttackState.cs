using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : State<MyGameStateData>
{
    public override void OnInitialize()
    {
    }

    public override void Tick()
    {
        Debug.Log("Attacking now!!");
    }

    public override void OnTransitionEnter()
    {
    }

    public override void OnTransitionExit()
    {
    }
}
