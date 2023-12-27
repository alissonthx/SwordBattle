using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private string IS_WALKING = "isWalking";
    [SerializeField] private string IS_RUNING = "isRunning";


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAnimation();
    }

    private void HandleAnimation()
    {
        bool isWalking = animator.GetBool(IS_WALKING);
        bool isRunning = animator.GetBool(IS_RUNING);

        if (gameInput.IsMovementPressed() && !isWalking)
        {
            animator.SetBool(IS_WALKING, true);
        }
        else if (!gameInput.IsMovementPressed() && isWalking)
        {
            animator.SetBool(IS_WALKING, false);
        }

        if (gameInput.IsMovementPressed() && gameInput.IsRunPressed() && !isRunning)
        {
            animator.SetBool(IS_RUNING, true);
        }
    }
}
