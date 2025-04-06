using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState,BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> currentState;
    protected bool isTransitioningState = false;

    private void Start()
    {
        currentState.EnterState();  
    }

    private void Update()
    {
        EState nextState = currentState.GetNextState();
        if (!isTransitioningState && nextState.Equals(currentState.Statekey))
        {
            currentState.UpdateState();
        }
        else if (!isTransitioningState)
        {
            TransitionToState(nextState);
        }
    }

    public void TransitionToState(EState state)
    {
        isTransitioningState = true;
        currentState.ExitState();
        currentState = States[state];
        currentState.EnterState();
        isTransitioningState = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
    private void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }
    private void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }
}
