using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

//The player inventory
public class Inventory : MonoBehaviour, ArrayDataInterface
{
    [SerializeField] private Interactor interactor;
    private PlayThroughData playThroughData;

    [SerializeField] private PlayerBehaviour playerBehaviour;
    private PlayerAnimMode animMode;

    [SerializeField] private Image[] highLight;//holds highlights for the slots
    [SerializeField] private Image[] slots;//hold item images in runtime to be accessed

    [SerializeField] private GameObject[] items;//holds items to be instantiated
    [SerializeField] private Sprite[] icons;//holds icons to be displayed

    [SerializeField] private ArmAnimBehaviour armAnimBehaviour;

    private GameObject[] heldItems = new GameObject[5];//holds items in runtime
    private ItemEnum[] itemEnums = new ItemEnum[5];

    private int activeSlot;
    private int lastActiveSlot;

    public void LoadData(ArrayGameData data)
    {
        itemEnums = data.invItemEnums;
        int slot = 0;
        foreach(ItemEnum itemEnum in itemEnums)
        {
            if (itemEnum != ItemEnum.Empty)
            {
                bool activeItem = false;
                int ID = ((int)itemEnum) - 1;
                if (slot == 0)
                {
                    activeItem = true;
                }
                AddItemStandard(activeItem, ID, slot);
            }
            slot++;
        }
    }

    public void SaveData(ref ArrayGameData data)
    {
        int slot = 0;
        foreach (GameObject item in heldItems)
        {
            itemEnums[slot] = GetItemEnum(item);
            slot++;
        }
        data.invItemEnums = itemEnums;
    }

    void Start()
    {
        playThroughData = FindFirstObjectByType<PlayThroughData>();
        activeSlot = 0;
        lastActiveSlot = 0;
        highLight[activeSlot].enabled = true;
    }

    void Update()
    {
        animMode = playerBehaviour.GetAnimMode();
        if (animMode == PlayerAnimMode.Jump || animMode == PlayerAnimMode.Run || playerBehaviour.paused)
        {
            return;
        }
        if (armAnimBehaviour.armsLocked)
        {
            return;
        }

        lastActiveSlot = activeSlot;


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeSlot = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeSlot = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeSlot = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            activeSlot = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            activeSlot = 4;
        }
        else if (Input.GetKeyDown(KeyCode.R) && heldItems[activeSlot] != null)
        {
            RemoveItem(activeSlot);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if(scroll > 0)
        {
            if (activeSlot == 4)
            {
                activeSlot = 0;
            }
            else
            {
                activeSlot++;
            }
        }
        else if(scroll < 0)
        {
            if(activeSlot == 0)
            {
                activeSlot = 4;
            }
            else
            {
                activeSlot--;
            }
        }

        
        if (activeSlot != lastActiveSlot)
        {
            armAnimBehaviour.ActivateForceSwitch();
            highLight[activeSlot].enabled = true;
            highLight[lastActiveSlot].enabled = false;

            if (heldItems[lastActiveSlot] != null)
            {
                heldItems[lastActiveSlot].SetActive(false);
            }
            if (heldItems[activeSlot] != null)
            {
                interactor.enabled = false;
                heldItems[activeSlot].SetActive(true);
            }
            else
            {
                interactor.enabled = true;
            }
        }
    }

    public void RemoveItem(int slot)
    {
        Destroy(heldItems[slot]);
        armAnimBehaviour.ActivateForceSwitch();
        slots[slot].enabled = false;
        slots[slot].sprite = null;
        interactor.enabled = true;
    }

    public void RemoveItemAfterUsed(int slot)
    {
        Destroy(heldItems[slot]);
        armAnimBehaviour.ActivateForceSwitch();
        slots[slot].enabled = false;
        slots[slot].sprite = null;
        interactor.enabled = true;
        playThroughData.IncrementItems();
    }

    public ItemEnum GetCurrentItem()
    {
        ItemEnum currentItem = ItemEnum.Empty;
        
        if (HasAnItem())
        {
            currentItem = heldItems[activeSlot].gameObject.GetComponent<Item>().GetItem();
        }
        return currentItem;
    }

    public bool HasAnItem()
    {
        return heldItems[activeSlot] != null;
    }

    public void RotateCurrentItem(Quaternion rotation)
    {
        heldItems[activeSlot].transform.rotation = rotation;
    }

    
    public IEnumerator AddItem(string tag)
    {
        int slot = activeSlot;
        int ID = -1;

        switch (tag)
        {
            case "AirHorn":
                ID = 0;
                break;
            case "Shovel":
                ID = 1;
                break;
            case "Gun":
                ID = 2;
                break;
            case "Vicodin":
                ID = 3;
                break;
        }

        try
        {
            bool activeItem = true; 
            AddItemStandard(activeItem, ID, slot);
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError(e.Message);
        }
        catch (NullReferenceException e)
        {
            Debug.LogError(e.Message);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        yield return null;
    }
    
    private void AddItemStandard(bool activeItem, int ID, int slot)
    {
        GameObject item = Instantiate(items[ID], gameObject.transform);
        item.SetActive(activeItem);
        item.GetComponent<Item>().SetSlot(slot);
        heldItems[slot] = item;
        slots[slot].enabled = true;
        slots[slot].sprite = icons[ID];
        interactor.enabled = false;
        armAnimBehaviour.ActivateForceSwitch();
    }

    private ItemEnum GetItemEnum(GameObject item)
    {
        ItemEnum itemEnum = ItemEnum.Empty;

        if (item != null)
        {
            itemEnum = item.GetComponent<Item>().GetItem();
        }
        return itemEnum;
    }
}
