using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private InputControls inputControlls;

    private void Awake()
    {
        Instance = this;
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputControlls.PlayerControls.Movement.ReadValue<Vector2>();

        return inputVector;
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
