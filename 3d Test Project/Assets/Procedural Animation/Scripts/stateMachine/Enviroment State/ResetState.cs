using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetState : EnviromentInteractionState
{
    float _elspsedTime = 0;
    float _restDuration = 2;
    //float _lerpDuraction = 10f;

    bool isMoving;
    public ResetState(EnviromentInteractionContex contex, EnviromentInteractionStateMachine.EEnviromentInteractionState stateKey)
        : base(contex, stateKey)
    {
        EnviromentInteractionContex Contex = contex;
    }

    public override void EnterState()
    {
        _elspsedTime = 0;
        Contex.closestPointOnColliderFromSolder = Vector3.positiveInfinity;
        Contex.currentIntercetingCollider = null;
    }
    public override void ExitState() { }
    public override void UpdateState()
    {
        _elspsedTime += Time.deltaTime;
        //Contex.InteractionPointYOffset = Mathf.Lerp(Contex.InteractionPointYOffset,Contex.ColliderCenterY, _elspsedTime/_lerpDuraction);
    }
    public override EnviromentInteractionStateMachine.EEnviromentInteractionState GetNextState()
    {
        isMoving = Contex.Rigidbody.velocity != Vector3.zero;
        if (_elspsedTime >= _restDuration && isMoving)
        {
            _elspsedTime = 0;
            return EnviromentInteractionStateMachine.EEnviromentInteractionState.Serach;
        }
        return Statekey;
    }
    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
