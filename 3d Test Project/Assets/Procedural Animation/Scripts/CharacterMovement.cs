using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float rotationSpeed = 10f;

    private Rigidbody rb;
    private Animator animator;
    private Vector3 lastMoveDirection = Vector3.zero; // Store last direction to prevent stopping after turn

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.isKinematic = true; // Use MovePosition for movement
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        bool isMoving = direction.magnitude > 0;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        // Animation Handling
        animator.SetBool("isWalking", isMoving && !isRunning);
        animator.SetBool("isRunning", isRunning);

        if (isMoving)
        {
            float speed = isRunning ? runSpeed : walkSpeed;

            // Convert movement direction to world space
            Vector3 move = transform.forward * vertical + transform.right * horizontal;
            lastMoveDirection = move.normalized; // Store last valid direction

            // Calculate target position while preserving the y-position
            Vector3 targetPosition = rb.position + lastMoveDirection * speed * Time.deltaTime;
            targetPosition.y = rb.position.y; // Preserve Y-position to avoid halting movement

            // Move character
            rb.MovePosition(targetPosition);

            // Rotate Character
            Quaternion toRotation = Quaternion.LookRotation(lastMoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else if (lastMoveDirection != Vector3.zero)
        {
            // Maintain previous movement direction for smoother turning
            Quaternion toRotation = Quaternion.LookRotation(lastMoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
