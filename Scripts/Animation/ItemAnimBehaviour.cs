
using UnityEngine;

//Handles item animation logic
public class ItemAnimBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void SetNeutral(bool state)
    {
        animator.SetBool("Neutral", state);
    }

    public void SetWalk(bool state)
    {
        animator.SetBool("Walk", state);
    }

    public void SetJump(bool state)
    {
        animator.SetBool("Jump", state);
    }

    public void SetRun(bool state)
    {
        animator.SetBool("Run", state);
    }

    public void SetAction()
    {
        animator.SetTrigger("Action");
    }
}
