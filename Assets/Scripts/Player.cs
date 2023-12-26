using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private CharacterController characterController;
    private InputControls inputControls;
    private GameInput gameInput;

    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    private bool movementPressed;
    private bool runPressed;
    private bool isWalking;
    [SerializeField] private float rotationFactorPerFrame = 15f;
    [SerializeField] private float runSpeed = 3f;
    [SerializeField] private float moveSpeed = 15f;

    private void Awake()
    {
        Instance = this;
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        characterController.Move(currentMovement * Time.deltaTime);
    }

    private void Update()
    {
        HandleGravity();
        HandleMovement();
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        runPressed = context.ReadValueAsButton();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float playerRadius = 2f;
        float playerHeight = 6f;
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;

        if (!canMove)
        {
            // Cannot move towards moveDir
            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -0.5f || moveDir.x > +0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // can move only on the X 
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -0.5f || moveDir.z > +0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // can move only on the Z
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move any direction
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    private void HandleGravity()
    {
        if (characterController.isGrounded)
        {
            float groundedGravity = -0.05f;
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else
        {
            float gravity = -9.8f;
            currentMovement.y += gravity;
        }
    }

    public bool IsMovementPressed()
    {
        return movementPressed;
    }

    public bool IsRunPressed()
    {
        return runPressed;
    }
}
