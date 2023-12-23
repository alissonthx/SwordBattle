using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private CharacterController characterController;
    private InputControls inputControls;

    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private bool movementPressed;
    private bool runPressed;
    private float rotationFactorPerFrame = 1f;

    private void Awake()
    {
        Instance = this;

        characterController = GetComponent<CharacterController>();

        inputControls = new InputControls();

        // Movement
        inputControls.PlayerControls.Movement.started += OnMovementInput;
        inputControls.PlayerControls.Movement.performed += OnMovementInput;
        inputControls.PlayerControls.Movement.canceled += OnMovementInput;

        // Run
        inputControls.PlayerControls.Run.performed += context => runPressed = context.ReadValueAsButton();
    }

    private void FixedUpdate()
    {
        characterController.Move(currentMovement * Time.deltaTime);
    }

    private void Update()
    {
        // Debug.Log("currentMovement: " + currentMovement + "  " + "movementPressed: " + movementPressed + "  " + "runPressed: " + runPressed);

        HandleRotation();
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        movementPressed = currentMovement.x != 0 || currentMovement.z != 0;
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;

        // change in position to player should point to
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = currentMovement.z;
        // the current rotation of player
        Quaternion currentRotation = transform.rotation;

        if (movementPressed)
        {
            // creates a new rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);

        }
    }

    private void OnEnable()
    {
        inputControls.Enable();
    }
    private void OnDisable()
    {
        inputControls.Disable();
    }

    public bool IsMovementPressed()
    {
        return movementPressed;
    }
}
