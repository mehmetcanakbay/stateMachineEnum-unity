using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract void Tick(StateDataInjection data);
    public abstract void OnTransitionEnter(StateDataInjection data);
    public abstract void OnTransitionExit(StateDataInjection data);
    public abstract void OnInitialize(StateDataInjection data);
}
