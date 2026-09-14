
using UnityEngine;

//Arm side logic for syncing item animations
public class ArmAnimMachine : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (FindFirstObjectByType<ItemAnimBehaviour>(FindObjectsInactive.Exclude)!=null)
        {
            animator.SetBool("ChangedState", true);
        }
    }
}
