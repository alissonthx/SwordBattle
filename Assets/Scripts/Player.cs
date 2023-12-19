using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Animator animator;
    public string IS_RUNING = "isWalking";
    public string IS_WALKING = "isRunning";
    private InputControls inputControls;

    private Vector2 currentMovement;
    private bool movementPressed;
    private bool runPressed;

    private void Awake()
    {
        inputControls = new InputControls();

        inputControls.PlayerControls.Movement.performed += ctx =>
        {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };

        inputControls.PlayerControls.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        // Debug.Log("currentMovement: " + currentMovement + "  " + "movementPressed: " + movementPressed + "  " + "runPressed: " + runPressed);

        HandleMovement();
        HandleRotation();
    }

    private void HandleRotation()
    {
        Vector3 currentPos = transform.position;
        Vector3 newPos = new Vector3(currentMovement.x, 0, currentMovement.y);
        Vector3 positionToLookAt = currentPos + newPos;

        transform.LookAt(positionToLookAt);
    }

    private void HandleMovement()
    {
        
    }

    private void OnEnable()
    {
        inputControls.Enable();
    }
    private void OnDisable()
    {
        inputControls.Disable();
    }
}
