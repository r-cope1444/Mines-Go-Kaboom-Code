
using UnityEngine;

public enum ItemEnum
{
    Empty,
    AirHorn,
    Shovel,
    Gun,
    Vicodin
}


//Parent class for the different item behaviours.
public class Item : MonoBehaviour
{
    protected int itemSlot;
    protected ItemEnum itemEnum;
    protected PlayerBehaviour playerBehaviour;
    protected PlayerAnimMode animMode;
    protected ArmAnimBehaviour armAnimBehaviour;

    public void SetSlot(int slot)
    {
        itemSlot = slot;
    }

    public ItemEnum GetItem()
    {
        return itemEnum;
    }

    virtual protected void GetPlayerComponents()
    {
        playerBehaviour = gameObject.GetComponentInParent<PlayerBehaviour>();
        armAnimBehaviour = playerBehaviour.gameObject.GetComponentInChildren<ArmAnimBehaviour>();
        animMode = playerBehaviour.GetAnimMode();
    }
}
