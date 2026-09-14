using UnityEngine;

//Determines whether the items in the scene have been picked up or not.
public class PickupItemCounter : MonoBehaviour, ArrayDataInterface
{
    [SerializeField] private PickupItem[] items;
    private bool[] inactiveItems; 

    public void LoadData(ArrayGameData data)
    { 
        inactiveItems = data.inactiveItems;
        int ID = 0;

        foreach (PickupItem item in items)
        {
            item.SetID(ID);
            if (inactiveItems[ID])
            {
                item.gameObject.SetActive(false);
            }
            ID++;
        }
    }

    public void SaveData(ref ArrayGameData data)
    {
        data.inactiveItems = inactiveItems;
    }

    public void DisableItem(int ID)
    {
        inactiveItems[ID] = true;
    }
}
