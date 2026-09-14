using System.Collections;
using UnityEngine;

//Behaviour for the vicodin pills. Makes player immortal for a few seconds via playerRagDoll.StartDruggedCoroutine();
public class Drug : Item
{
    [SerializeField] private ItemAnimBehaviour itemAnimBehaviour;
    [SerializeField] private AudioSource nomSound;

    private PlayerRagdoll playerRagDoll;
    private Inventory inventory;

    private bool canDoDrug;

    private float animTime;

    void Start()
    {
        canDoDrug = true;
        playerRagDoll = FindFirstObjectByType<PlayerRagdoll>();
        inventory = FindFirstObjectByType<Inventory>();
        GetPlayerComponents();
        itemEnum = ItemEnum.Vicodin;
    }

    
    void Update()
    {
        if (!canDoDrug)
        {
            animTime = armAnimBehaviour.GetAnimTime();
        }

        animMode = playerBehaviour.GetAnimMode();

        if (animMode == PlayerAnimMode.Run || animMode == PlayerAnimMode.Jump || playerBehaviour.paused)
        {
            return;
        }
        if (!canDoDrug)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            armAnimBehaviour.SetTrigger("DrugUse", itemAnimBehaviour);
            StartCoroutine(EatDrug());
            canDoDrug = false;
        }
    }

    private IEnumerator EatDrug()
    {
        yield return new WaitUntil(() => animTime >= 0.7f);
        nomSound.Play();
        yield return new WaitUntil(() => animTime >= 0.85f);
        animTime = 0f;
        armAnimBehaviour.ActivateForceSwitch();
        playerRagDoll.StartDruggedCoroutine();
        inventory.RemoveItemAfterUsed(itemSlot);
    }
}
