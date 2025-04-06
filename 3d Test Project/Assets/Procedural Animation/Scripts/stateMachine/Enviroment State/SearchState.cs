using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : EnviromentInteractionState
{
    public SearchState(EnviromentInteractionContex contex, EnviromentInteractionStateMachine.EEnviromentInteractionState stateKey) : base(contex, stateKey)
    {
        EnviromentInteractionContex Contex = contex;
    }

    public override void EnterState() 
    {
        Debug.Log("Enter Search State");
    }
    public override void ExitState() { }
    public override void UpdateState() { }
    public override EnviromentInteractionStateMachine.EEnviromentInteractionState GetNextState()
    {
        return Statekey;
    }
    public override void OnTriggerEnter(Collider other) 
    {
        Debug.LogError("Trigger Search entered ");
        StartIKTargatePositionTracking(other);
    }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
