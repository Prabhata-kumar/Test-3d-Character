using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ProceduralWallInteraction : MonoBehaviour
{
    public enum InteractionState
    {
        Search,
        Approach,
        Raise,
        Touch,
        Reset
    }

    [Header("Rigging & IK")]
    [SerializeField] private TwoBoneIKConstraint _leftIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightIKConstraint;
    [SerializeField] private MultiRotationConstraint _leftMultiAimConstraint;
    [SerializeField] private MultiRotationConstraint _rightMultiAimConstraint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private float reachThreshold = 1f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("IK Targets")]
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    private InteractionState _state = InteractionState.Search;
    private Transform currentTarget;
    private Vector3 currentIKTargetPosition;
    private float touchHoldDuration = 1.0f;
    private float touchTimer = 0f;

    private void Update()
    {
        switch (_state)
        {
            case InteractionState.Search:
                HandleSearch();
                break;
            case InteractionState.Approach:
                HandleApproach();
                break;
            case InteractionState.Raise:
                HandleRaise();
                break;
            case InteractionState.Touch:
                HandleTouch();
                break;
            case InteractionState.Reset:
                HandleReset();
                break;
        }
    }

    private void HandleSearch()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, interactableLayer);
        float closestDist = float.MaxValue;
        Transform closest = null;

        foreach (var hit in hits)
        {
            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = hit.transform;
            }
        }

        if (closest != null)
        {
            currentTarget = closest;
            _state = InteractionState.Approach;
        }
    }

    private void HandleApproach()
    {
        if (!currentTarget) { _state = InteractionState.Reset; return; }

        Vector3 dir = (currentTarget.position - transform.position).normalized;
        transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * 5f);
        currentIKTargetPosition = currentTarget.position;

        if (Vector3.Distance(transform.position, currentTarget.position) <= reachThreshold)
        {
            _state = InteractionState.Raise;
        }
    }

    private void HandleRaise()
    {
        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = (currentTarget.position - origin).normalized;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, reachThreshold, interactableLayer))
        {
            currentIKTargetPosition = hit.point;
            Quaternion alignRotation = Quaternion.LookRotation(-hit.normal);
            leftHandTarget.position = hit.point;
            leftHandTarget.rotation = alignRotation;
            _leftIKConstraint.weight = Mathf.MoveTowards(_leftIKConstraint.weight, 1f, Time.deltaTime * 5f);

            if (Vector3.Distance(leftHandTarget.position, hit.point) < 0.05f)
            {
                _state = InteractionState.Touch;
                touchTimer = 0f;
            }
        }
        else
        {
            _state = InteractionState.Reset;
        }
    }

    private void HandleTouch()
    {
        touchTimer += Time.deltaTime;
        if (touchTimer >= touchHoldDuration)
        {
            _state = InteractionState.Reset;
        }
    }

    private void HandleReset()
    {
        _leftIKConstraint.weight = Mathf.MoveTowards(_leftIKConstraint.weight, 0f, Time.deltaTime * 3f);
        if (_leftIKConstraint.weight <= 0.01f)
        {
            currentTarget = null;
            currentIKTargetPosition = Vector3.zero;
            _state = InteractionState.Search;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (currentTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(currentTarget.position, 0.15f);
        }

        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(currentIKTargetPosition, Vector3.one * 0.05f);
        Gizmos.DrawLine(transform.position, currentIKTargetPosition);
    }
}
