using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private string IS_WALKING = "isWalking";
    [SerializeField] private string IS_RUNNING = "isRunning";
    // [SerializeField] private string INPUT_MAGNITUDE = "InputMagnitude";

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
        bool isRunning = anim.GetBool(IS_RUNNING);

        if (gameInput.IsMovementPressed() && !isWalking)
        {
            anim.SetBool(IS_WALKING, true);
            // anim.SetFloat(INPUT_MAGNITUDE, gameInput.GetInputMagnitude() * Player.Instance.GetAcceleration(), .1f, Time.deltaTime);
        }
        else if (!gameInput.IsMovementPressed() && isWalking)
        {
            anim.SetBool(IS_WALKING, false);
        }

        else if (gameInput.IsMovementPressed() && gameInput.IsRunPressed() && !isRunning)
        {
            anim.SetBool(IS_RUNNING, true);
        }

        else if (!gameInput.IsRunPressed() && isRunning)
        {
            anim.SetBool(IS_RUNNING, false);
        }
    }

    // private void OnDisable()
    // {
    //     anim.SetFloat("InputMagnitude", 0);
    // }
}
