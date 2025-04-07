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
        if (intersectingCollider.gameObject.layer == LayerMask.NameToLayer("Interactable") &&
            Contex.currentIntercetingCollider == null)
        {
            Contex.currentIntercetingCollider = intersectingCollider;
            Vector3 getClosestPointFormRoot = GetClosserPointCollider(intersectingCollider, Contex.RootTransform.position);
            Contex.SetCurrentSide(getClosestPointFormRoot);
            SetIkTergatPosition();
        }
    }

    protected void UpdateIKTargatePosition(Collider intersectingCollider)
    {
        if(intersectingCollider == Contex.currentIntercetingCollider)
        {

            SetIkTergatPosition();
        }
    }

    protected void ResetIKTargatePosationTracking(Collider intersectingCollider)
    {
        if(intersectingCollider == Contex.currentIntercetingCollider)
        {
            Contex.currentIntercetingCollider = null;
            Contex.closestPointOnColliderFromSolder = Vector3.positiveInfinity;
        }
    }

    public void SetIkTergatPosition()
    {
       
        Contex.closestPointOnColliderFromSolder = GetClosserPointCollider(Contex.currentIntercetingCollider,
            Contex.currentSolderTransfoem.position);
        new Vector3(Contex.currentSolderTransfoem.position.x, Contex.characterSholderHight.y, Contex.currentSolderTransfoem.position.z);
    }
}
