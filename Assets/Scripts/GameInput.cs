using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private InputControls inputControlls;
    private Vector2 inputVector;
    private bool runPressed;
    private bool movementPressed;

    private void Awake()
    {
        Instance = this;

        inputControlls = new InputControls();
    }

    public float GetInputMagnitude()
    {
        float inputMagnitude = new Vector2(GetMovementVectorNormalized().x, GetMovementVectorNormalized().y).sqrMagnitude;
        return inputMagnitude;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        runPressed = context.ReadValueAsButton();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        inputVector = inputControlls.PlayerControls.Movement.ReadValue<Vector2>();

        return inputVector;
    }

    public bool IsMovementPressed()
    {
        return movementPressed = inputVector.x != 0 || inputVector.y != 0;
    }

    public bool IsRunPressed()
    {
        return runPressed;
    }

    private void OnEnable()
    {
        inputControlls.PlayerControls.Enable();
    }

    private void OnDisable()
    {
        inputControlls.PlayerControls.Disable();
    }

}
