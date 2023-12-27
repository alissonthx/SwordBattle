using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private string IS_WALKING = "isWalking";
    [SerializeField] private string IS_RUNING = "isRunning";
    [SerializeField] private string INPUT_MAGNITUDE = "InputMagnitude";

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAnimation();
    }

    private void HandleAnimation()
    {
        bool isWalking = anim.GetBool(IS_WALKING);
        bool isRunning = anim.GetBool(IS_RUNING);

        if (gameInput.IsMovementPressed() && !isWalking)
        {
            anim.SetBool(IS_WALKING, true);
            anim.SetFloat(INPUT_MAGNITUDE, gameInput.GetInputMagnitude() * Player.Instance.GetAcceleration(), .1f, Time.deltaTime);
        }
        else if (!gameInput.IsMovementPressed() && isWalking)
        {
            anim.SetBool(IS_WALKING, false);
        }

        if (gameInput.IsMovementPressed() && gameInput.IsRunPressed() && !isRunning)
        {
            anim.SetBool(IS_RUNING, true);
        }
    }

    private void OnDisable()
    {
        anim.SetFloat("InputMagnitude", 0);
    }
}
