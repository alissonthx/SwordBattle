using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
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

        if (Player.Instance.IsMovementPressed() && !isWalking)
        {
            animator.SetBool(IS_WALKING, true);
        }
        else if (!Player.Instance.IsMovementPressed() && isWalking)
        {
            animator.SetBool(IS_WALKING, false);
        }

        if (Player.Instance.IsMovementPressed() && Player.Instance.IsRunPressed() && !isRunning)
        {
            animator.SetBool(IS_RUNING, true);
        }
    }
}
