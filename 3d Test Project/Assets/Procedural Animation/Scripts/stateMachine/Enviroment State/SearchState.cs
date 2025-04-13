using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : EnviromentInteractionState
{
    bool isCloseToTargate;
    bool isClosestPointOnColliderValid;
    float _approchDisstanceThreshold = 2.0f;
    public SearchState(EnviromentInteractionContex contex, EnviromentInteractionStateMachine.EEnviromentInteractionState stateKey) : base(contex, stateKey)
    {
        EnviromentInteractionContex Contex = contex;
    }

    public override void EnterState()
    {
        Debug.LogError("Search state entered ");
    }
    public override void ExitState() { }
    public override void UpdateState()
    {

    }
    public override EnviromentInteractionStateMachine.EEnviromentInteractionState GetNextState()
    {
        isCloseToTargate = Vector3.Distance(Contex.closestPointOnColliderFromSolder,
            Contex.RootTransform.position) < _approchDisstanceThreshold;
        isClosestPointOnColliderValid = Contex.closestPointOnColliderFromSolder != Vector3.positiveInfinity;
        if (isCloseToTargate && isClosestPointOnColliderValid)
        {
            return EnviromentInteractionStateMachine.EEnviromentInteractionState.Aproch;
        }
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

