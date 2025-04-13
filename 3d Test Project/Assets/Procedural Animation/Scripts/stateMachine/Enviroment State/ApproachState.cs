using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachState : EnviromentInteractionState
{
    float _elapsedTime = 0;
    float _lerpDuraction = 5.0f;
    float _approchWeight = 0.5f;
    public ApproachState(EnviromentInteractionContex contex, EnviromentInteractionStateMachine.EEnviromentInteractionState stateKey) : base(contex, stateKey)
    {
        EnviromentInteractionContex Contex = contex;
    }

    public override void EnterState()
    {
        Debug.Log("Enter approch State");
        _elapsedTime = 0;
    }
    public override void ExitState() { }
    public override void UpdateState()
    {
        _elapsedTime += Time.deltaTime;
        //Contex.currentIKContrain.weight = Mathf.Lerp(Contex.currentIKContrain.weight, _approchWeight, _elapsedTime / _lerpDuraction);
    }
    public override EnviromentInteractionStateMachine.EEnviromentInteractionState GetNextState()
    {
        return Statekey;
    }
    public override void OnTriggerEnter(Collider other)
    {

        StartIKTargatePositionTracking(other);
    }
    public override void OnTriggerStay(Collider other)
    {
        UpdateIKTargatePosition(other);
    }
    public override void OnTriggerExit(Collider other)
    {
        ResetIKTargatePosationTracking(other);
    }
}
