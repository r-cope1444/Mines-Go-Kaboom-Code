
using UnityEngine;

//Item side logic for syncing animations
public class ItemAnimMachine : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var animsync = FindFirstObjectByType<ArmAnimBehaviour>().gameObject.GetComponent<Animator>();
        if (animsync.GetBool("ChangedState"))
        {
            animator.SetFloat("Offset", animsync.GetNextAnimatorStateInfo(0).normalizedTime % 1f);
            animsync.SetBool("ChangedState", false);
        }
        else
        {
            animator.SetFloat("Offset", animsync.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f);
        }
    }
}
