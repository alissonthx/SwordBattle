using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private Animator anim;
    private Camera cam;
    private CharacterController controller;

    private Vector3 desiredMoveDirection;
    private Vector3 moveVector;
    private float verticalVel;
    [SerializeField] private GameInput gameInput;

    [Header("Settings")]
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed = 0.1f;
    [SerializeField] float fallSpeed = .2f;
    public float acceleration = 1;

    [Header("Booleans")]
    [SerializeField] bool blockRotationPlayer;
    private bool isGrounded;
    private Vector3 currentRunMovement;

    private void Awake()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main;
    }

    private void Update()
    {
        InputMagnitude();

        isGrounded = controller.isGrounded;

        if (isGrounded)
            verticalVel -= 0;
        else
            verticalVel -= 1;

        moveVector = new Vector3(0, verticalVel * fallSpeed * Time.deltaTime, 0);
        controller.Move(moveVector);
    }

    public void LookAt(Vector3 pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), rotationSpeed);
    }

    public void RotateToCamera(Transform t)
    {
        var forward = cam.transform.forward;

        desiredMoveDirection = forward;
        Quaternion lookAtRotation = Quaternion.LookRotation(desiredMoveDirection);
        Quaternion lookAtRotationOnly_Y = Quaternion.Euler(transform.rotation.eulerAngles.x, lookAtRotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        t.rotation = Quaternion.Slerp(transform.rotation, lookAtRotationOnly_Y, rotationSpeed);
    }

    void InputMagnitude()
    {
        //Calculate the Input Magnitude
        float inputMagnitude = new Vector2(gameInput.GetMovementVectorNormalized().x, gameInput.GetMovementVectorNormalized().y).sqrMagnitude;

        //Physically move player
        if (inputMagnitude > 0.1f)
        {
            anim.SetFloat("InputMagnitude", inputMagnitude * acceleration, .1f, Time.deltaTime);
            HandleMovement();
        }
        else
        {
            anim.SetFloat("InputMagnitude", inputMagnitude * acceleration, .1f, Time.deltaTime);
        }
    }

    private void HandleMovement()
    {
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * gameInput.GetMovementVectorNormalized().y + right * gameInput.GetMovementVectorNormalized().x;

        if (blockRotationPlayer == false)
        {
            //Camera
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed * acceleration);
            controller.Move(desiredMoveDirection * Time.deltaTime * (movementSpeed * acceleration));
        }
        else
        {
            //Strafe
            controller.Move((transform.forward * gameInput.GetMovementVectorNormalized().y + transform.right * gameInput.GetMovementVectorNormalized().y) * Time.deltaTime * (movementSpeed * acceleration));
        }
    }

    private void OnDisable()
    {
        anim.SetFloat("InputMagnitude", 0);
    }
}
