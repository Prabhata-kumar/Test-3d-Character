using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState,BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> currentState;  

    private void Start()
    {
        currentState.EnterState();  
    }

    private void Update()
    {
        currentState.UpdateState();
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
