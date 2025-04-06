using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnviromentInteractionContex : MonoBehaviour
{
    public enum EBodySide
    {
        Left,
        Right 
    };

    private TwoBoneIKConstraint _leftIKConstraint;
    private TwoBoneIKConstraint _rightIKConstraint;
    private MultiRotationConstraint _leftMultiAimConstraint;
    private MultiRotationConstraint _rightMultiAimConstraint;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private Transform _rootTransform;

    public EnviromentInteractionContex(TwoBoneIKConstraint leftIKConstraint, TwoBoneIKConstraint rightIKConstraint,
        MultiRotationConstraint leftMultiAimConstraint, MultiRotationConstraint rightMultiAimConstraint, 
        Rigidbody rigidbody, Collider collider, Transform rootTransform)
    {
        _leftIKConstraint = leftIKConstraint;
        _rightIKConstraint = rightIKConstraint;
        _leftMultiAimConstraint = leftMultiAimConstraint;
        _rightMultiAimConstraint = rightMultiAimConstraint;
        _rigidbody = rigidbody;
        _collider = collider;
        _rootTransform = rootTransform;
    }

    public TwoBoneIKConstraint LeftIKConstraint => _leftIKConstraint;
    public TwoBoneIKConstraint RightIKConstraint => _rightIKConstraint;
    public MultiRotationConstraint LeftMultiAimConstraint => _leftMultiAimConstraint;
    public MultiRotationConstraint RightMultiAimConstraint => _rightMultiAimConstraint;
    public Rigidbody Rigidbody => _rigidbody;
    public Collider Collider => _collider;
    public Transform RootTransform => _rootTransform;

    public TwoBoneIKConstraint currentIKContrain { get; private set; }
    public MultiRotationConstraint currentMultiRotationConstraint { get; private set; } 
    public Transform currentIKTargateTransform { get; private set; }
    public Transform currentSolderTransfoem {  get; private set; }
    public EBodySide currentBodySide { get; private set; }

    public void SetCurrentSide(Vector3 positionToCheck)
    {
        Vector3 leftShoulder = _leftIKConstraint.data.root.transform.position;
        Vector3 rightSholder = _rightIKConstraint.data.root.transform.position;

        bool isleftClose = Vector3.Distance(positionToCheck,leftShoulder) < Vector3.Distance(positionToCheck,rightSholder);
        if (isleftClose)
        {
            currentBodySide = EBodySide.Left;
            currentIKContrain = _leftIKConstraint;
            currentMultiRotationConstraint = _leftMultiAimConstraint;
            Debug.Log("left side ");
        }
        else
        {
            currentBodySide = EBodySide.Right;
            currentIKContrain = _rightIKConstraint;
            currentMultiRotationConstraint = _rightMultiAimConstraint;
            Debug.Log("right side");
        }

        currentSolderTransfoem = currentIKContrain.data.root.transform; 
        currentIKTargateTransform = currentIKContrain.data.target.transform;
    }

}
