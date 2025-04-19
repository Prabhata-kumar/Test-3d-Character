using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiseState : EnviromentInteractionState
{
    float _elspsedTime = 0;
    float _lerpDuraction = 5f;
    Quaternion _expectedHandRotation;
    float _maxDistance = .5f;
    protected LayerMask _interactableLayerMask = LayerMask.GetMask("Interactable");
    float _rotationSpeed = 500f;
    public RiseState(EnviromentInteractionContex contex, EnviromentInteractionStateMachine.EEnviromentInteractionState stateKey) : base(contex, stateKey)
    {
        EnviromentInteractionContex Contex = contex;
    }

    public override void EnterState() 
    {
        Debug.LogError("entered in a raise state");
    }
    public override void ExitState() { }
    public override void UpdateState() 
    {
        CalculateExpectedHandRotation();

        Contex.interactionPointYOffset = Mathf.Lerp(Contex.interactionPointYOffset,
          Contex.colliderCenterY,
          _elspsedTime / _lerpDuraction);

        Contex.currentIKContrain.weight = Mathf.Lerp(Contex.currentMultiRotationConstraint.weight, 1, _elspsedTime / _lerpDuraction);
        Contex.currentMultiRotationConstraint.weight = Mathf.Lerp(Contex.currentMultiRotationConstraint.weight,
           1, _elspsedTime / _lerpDuraction);

        Contex.currentIKTargateTransform.rotation = Quaternion.RotateTowards(Contex.currentIKTargateTransform.rotation,
            _expectedHandRotation, _rotationSpeed * Time.deltaTime);

        _elspsedTime += Time.deltaTime;
    }

    private void CalculateExpectedHandRotation()
    {
        Vector3 startPos = Contex.currentSolderTransfoem.position;
        Vector3 endPos = Contex.closestPointOnColliderFromSolder;

        Vector3 direction = (endPos - startPos).normalized;

        RaycastHit hit;
        if (Physics.Raycast(startPos, direction, out hit, _maxDistance, _interactableLayerMask))
        {
            Vector3 surfaceNormal = hit.normal;

            // Align hand forward to point INTO the wall, using the normal
            Vector3 targetForward = -surfaceNormal;

            // Optional: Use surface up for more realistic rotation, fallback to Vector3.up
            Vector3 up = Vector3.up;

            _expectedHandRotation = Quaternion.LookRotation(targetForward, up);
        }
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
