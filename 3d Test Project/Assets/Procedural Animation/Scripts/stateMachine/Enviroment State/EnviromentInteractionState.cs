using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public abstract class EnviromentInteractionState : BaseState<EnviromentInteractionStateMachine.EEnviromentInteractionState>
{
    protected EnviromentInteractionContex Contex;

    public EnviromentInteractionState(EnviromentInteractionContex contex, EnviromentInteractionStateMachine.EEnviromentInteractionState 
        stateKey) :base(stateKey)
    {
        Contex = contex;
    }

    private Vector3 GetClosserPointCollider(Collider intersectingCollider,Vector3 positiontoCheck)
    {
        return intersectingCollider.ClosestPoint(positiontoCheck);
    }

    protected void StartIKTargatePositionTracking(Collider intersectingCollider)
    {
        Vector3 getClosestPointFormRoot = GetClosserPointCollider(intersectingCollider, Contex.RootTransform.position);
        Contex.SetCurrentSide(getClosestPointFormRoot);
    }

    protected void UpdateIKTargatePosition(Collider intersectingCollider)
    {

    }

    protected void ResetIKTargatePosationTracking(Collider intersectingCollider)
    {

    }
}
