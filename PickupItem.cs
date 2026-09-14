using UnityEngine;

//Behaviour for items that can be picked up (different to the items that are used)
public class PickupItem : MonoBehaviour
{
    private int itemID;
    private PickupItemCounter itemCounter;

    private void Start()
    {
        itemCounter = GetComponentInParent<PickupItemCounter>();
    }

    public void SetID(int ID)
    {
        itemID = ID;
    }

    public void DisableItem()
    {
        itemCounter.DisableItem(itemID);
        gameObject.SetActive(false);
    }
}
