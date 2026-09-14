
using UnityEngine;

//Handles arm animation logic
public class ArmAnimBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private Inventory inventory;

    public bool armsLocked;

    private AnimatorStateInfo stateInfo;

    private PlayerAnimMode currentAnimMode;
    private PlayerAnimMode lastAnimMode;
    private ItemEnum currentItem;
    private ItemEnum lastItem;

    private bool forceSwitch;

    void Update()
    {
        if (animator.IsInTransition(0))
        {
            stateInfo = animator.GetNextAnimatorStateInfo(0);
        }
        else
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        
        if (armsLocked)
        {
            return;
        }
        currentAnimMode = playerBehaviour.GetAnimMode();
        currentItem = inventory.GetCurrentItem();
        if (currentAnimMode != lastAnimMode || currentItem != lastItem || forceSwitch)
        {
            forceSwitch = false;
            ItemAnimBehaviour itemAnimBehaviour = null;
            if (inventory.GetComponentInChildren<ItemAnimBehaviour>(false) != null)
            {
                itemAnimBehaviour = inventory.GetComponentInChildren<ItemAnimBehaviour>(false);
            }
            DisableAllBools(itemAnimBehaviour);

            switch (currentAnimMode)//note that the vicodin, gun and airhorn reuse a lot of animations (named as gun animations)
            {
                case PlayerAnimMode.Neutral:
                    switch (currentItem)
                    {
                        case ItemEnum.Empty:
                            animator.SetBool("Idle", true);
                            break;
                        case ItemEnum.AirHorn:
                            animator.SetBool("DrugHornIdle", true);
                            break;
                        case ItemEnum.Shovel:
                            animator.SetBool("ShovelIdle", true);
                            break;
                        case ItemEnum.Gun:
                            animator.SetBool("GunIdle", true);
                            break;
                        case ItemEnum.Vicodin:
                            animator.SetBool("DrugHornIdle", true);
                            break;
                    }
                    if (itemAnimBehaviour != null)
                    {
                        itemAnimBehaviour.SetNeutral(true);
                    }
                    break;
                case PlayerAnimMode.Walk:
                    switch (currentItem)
                    {
                        case ItemEnum.Empty:
                            animator.SetBool("Walk", true);
                            break;
                        case ItemEnum.AirHorn:
                            animator.SetBool("GunWalk", true);
                            break;
                        case ItemEnum.Shovel:
                            animator.SetBool("ShovelWalk", true);
                            break;
                        case ItemEnum.Gun:
                            animator.SetBool("GunWalk", true);
                            break;
                        case ItemEnum.Vicodin:
                            animator.SetBool("GunWalk", true);
                            break;
                    }
                    if (itemAnimBehaviour != null)
                    {
                        itemAnimBehaviour.SetWalk(true);
                    }
                    break;
                case PlayerAnimMode.Run:
                    switch (currentItem)
                    {
                        case ItemEnum.Empty:
                            animator.SetBool("Run", true);
                            break;
                        case ItemEnum.AirHorn:
                            animator.SetBool("GunRun", true);
                            break;
                        case ItemEnum.Shovel:
                            animator.SetBool("ShovelRun", true);
                            break;
                        case ItemEnum.Gun:
                            animator.SetBool("GunRun", true);
                            break;
                        case ItemEnum.Vicodin:
                            animator.SetBool("GunRun", true);
                            break;
                    }
                    if (itemAnimBehaviour != null)
                    {
                        itemAnimBehaviour.SetRun(true);
                    }
                    break;
                case PlayerAnimMode.Jump:
                    switch (currentItem)
                    {
                        case ItemEnum.Empty:
                            animator.SetBool("Jump", true);
                            break;
                        case ItemEnum.AirHorn:
                            animator.SetBool("GunJump", true);
                            break;
                        case ItemEnum.Shovel:
                            animator.SetBool("ShovelJump", true);
                            break;
                        case ItemEnum.Gun:
                            animator.SetBool("GunJump", true);
                            break;
                        case ItemEnum.Vicodin:
                            animator.SetBool("GunJump", true);
                            break;
                    }
                    if (itemAnimBehaviour != null)
                    {
                        itemAnimBehaviour.SetJump(true);
                    }
                    break;
            }
        }
        
        lastAnimMode = currentAnimMode;
        lastItem = currentItem;
    }

    private void DisableArmBools()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("DrugHornIdle", false);
        animator.SetBool("ShovelIdle", false);
        animator.SetBool("GunIdle", false);
        animator.SetBool("Walk", false);
        animator.SetBool("GunWalk", false);
        animator.SetBool("ShovelWalk", false);
        animator.SetBool("Run", false);
        animator.SetBool("GunRun", false);
        animator.SetBool("ShovelRun", false);
        animator.SetBool("Jump", false);
        animator.SetBool("GunJump", false);
        animator.SetBool("ShovelJump", false);
    }

    private void DisableAllBools(ItemAnimBehaviour itemAnimBehaviour)
    {
        DisableArmBools();

        if (itemAnimBehaviour != null)
        {
            itemAnimBehaviour.SetNeutral(false);
            itemAnimBehaviour.SetWalk(false);
            itemAnimBehaviour.SetJump(false);
            itemAnimBehaviour.SetRun(false);
        }
    }

    public void SetTrigger(string name, ItemAnimBehaviour itemAnimBehaviour)
    {
        DisableAllBools(itemAnimBehaviour);
        animator.SetTrigger(name);
        if (itemAnimBehaviour != null)
        {
            inventory.GetComponentInChildren<ItemAnimBehaviour>(false).SetAction();
        }
    }

    public float GetAnimTime()
    {
        return stateInfo.normalizedTime;
    }

    public void ActivateForceSwitch()
    {
        forceSwitch = true;
    }
}
