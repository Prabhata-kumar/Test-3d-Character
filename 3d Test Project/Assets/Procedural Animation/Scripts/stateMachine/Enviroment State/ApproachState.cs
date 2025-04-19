using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ApproachState : EnviromentInteractionState
{
    float _elapsedTime = 0;
    float _lerpDuraction = 5.0f;
    float _approchWeight = 1f;
    float _approchDuraction = 2.0f;

    float _rotationSpeed = 500f;
    float _riseDistanceThreshold = 0.5f;
    public ApproachState(EnviromentInteractionContex contex, EnviromentInteractionStateMachine.EEnviromentInteractionState stateKey) : base(contex, stateKey)
    {
        EnviromentInteractionContex Contex = contex;
    }

    public override void EnterState()
    {
        Debug.LogError("Enter approch State");
        _elapsedTime = 0;
    }
    public override void ExitState() { }
    public override void UpdateState()
    {
        //make that forword rotation toward ground 
        quaternion excepetatedGroundRotation = Quaternion.Euler(new Vector3(0,0,-90));
        Contex.currentIKTargateTransform.rotation = Quaternion.RotateTowards(Contex.currentIKTargateTransform.rotation,
            excepetatedGroundRotation, _rotationSpeed * Time.deltaTime);
        Contex.currentMultiRotationConstraint.weight = Mathf.Lerp(Contex.currentMultiRotationConstraint.weight,
            0.75f, _elapsedTime / _lerpDuraction);
        _elapsedTime += Time.deltaTime;
        Contex.currentIKContrain.weight = Mathf.Lerp(Contex.currentIKContrain.weight,
            _approchWeight,
            _elapsedTime / _lerpDuraction);
    }
    public override EnviromentInteractionStateMachine.EEnviromentInteractionState GetNextState()
    {
        bool isOverStateLifeDuraction = _elapsedTime > _approchDuraction;
        if (isOverStateLifeDuraction) 
        {
            return EnviromentInteractionStateMachine.EEnviromentInteractionState.Reset;
        }
        bool isWithInAramsReach = Vector3.Distance(Contex.closestPointOnColliderFromSolder,
            Contex.currentSolderTransfoem.position) < _riseDistanceThreshold;
        bool isClosestPointIsColliderReal = Contex.closestPointOnColliderFromSolder != Vector3.positiveInfinity;
        if(isWithInAramsReach && isClosestPointIsColliderReal)
        {
            return EnviromentInteractionStateMachine.EEnviromentInteractionState.Rise;
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
