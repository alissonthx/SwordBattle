using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    private Camera cam;
    private CharacterController controller;

    private Vector3 desiredMoveDirection;
    private Vector3 moveVector;
    private float verticalVel;
    [SerializeField] private GameInput gameInput;

    [Header("Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private float fallSpeed = .2f;
    [SerializeField] private float acceleration = 1;

    [Header("Booleans")]
    [SerializeField] bool blockRotationPlayer;
    private bool isGrounded;

    private void Awake()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    private void Update()
    {
        //Physically move player
        if (gameInput.GetInputMagnitude() > 0.1f)
            HandleMovement();

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

    public void HandleMovement()
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

    public float GetAcceleration()
    {
        return acceleration;
    }
}
