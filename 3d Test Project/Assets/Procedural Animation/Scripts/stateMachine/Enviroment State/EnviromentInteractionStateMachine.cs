using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;

public class EnviromentInteractionStateMachine : StateManager<EnviromentInteractionStateMachine.EEnviromentInteractionState>
{
    public enum EEnviromentInteractionState
    {
        Serach,
        Aproch,
        Rise,
        Touch,
        Reset
    }

    [SerializeField] private TwoBoneIKConstraint _leftIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIKConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiAimConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiAimConstraint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    private EnviromentInteractionContex _contex;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if(_contex != null && _contex.closestPointOnColliderFromSolder != null)
        {
            Debug.Log("gizmosed drawned");
            Gizmos.DrawSphere(_contex.closestPointOnColliderFromSolder, 3f);
        }
    }
    private void Awake()
    {
        ValidationConstrain();
        _contex = new EnviromentInteractionContex(_leftIKConstraint, _rightIKConstraint,
            _leftMultiAimConstraint, _rightMultiAimConstraint, _rigidbody, _collider, transform.root);
        InisilizeStates();
    }

    private void ValidationConstrain()
    {
        Assert.IsNotNull(_leftIKConstraint, " _leftIKConstraint is empty");
        Assert.IsNotNull(_rightIKConstraint, " _rightIKConstraint is empty");
        Assert.IsNotNull(_leftMultiAimConstraint, " _leftMultiAimConstraint is empty");
        Assert.IsNotNull(_rightMultiAimConstraint, " _rightMultiAimConstraint is empty");
        Assert.IsNotNull(_rigidbody, " _rigidbody is empty");
        Assert.IsNotNull(_collider, " _collider is empty");
    }

    private void InisilizeStates()
    {
        States.Add(EEnviromentInteractionState.Reset, new ResetState(_contex, EEnviromentInteractionState.Reset));
        States.Add(EEnviromentInteractionState.Serach, new SearchState(_contex, EEnviromentInteractionState.Serach));
        States.Add(EEnviromentInteractionState.Aproch, new ApproachState(_contex, EEnviromentInteractionState.Aproch));
        States.Add(EEnviromentInteractionState.Rise, new RiseState(_contex, EEnviromentInteractionState.Rise));
        States.Add(EEnviromentInteractionState.Touch, new TouchState(_contex, EEnviromentInteractionState.Touch));
        currentState = States[EEnviromentInteractionState.Reset];
    }
}
