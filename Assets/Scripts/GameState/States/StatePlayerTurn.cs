using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MAKStateMachine;

public class StatePlayerTurn : State<GameStateData>
{
    public override void OnInitialize()
    {
    }

    public override void Tick()
    {
    }

    public override void OnTransitionEnter()
    {
        if (GameManager.Instance != null) {
            GameManager.Instance.EnablePlayerChoice();
        }
    }

    public override void OnTransitionExit()
    {
        if (GameManager.Instance != null) {
            GameManager.Instance.DisablePlayerChoice();
        }
    }
}
