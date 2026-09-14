using System.Collections;
using UnityEngine;

//Creates raycasts that blow away enemies
public class AirHorn : Item
{
    [SerializeField] private ItemAnimBehaviour itemAnimBehaviour;
    [SerializeField] private AudioSource hornSound;

    private bool canHorn;
    private bool hornUsed;
    private bool hornActive;

    private Looker looker;
    private Inventory inventory;

    private float hornPower;

    private float animTime;

    void Start()
    {
        canHorn = true;
        hornUsed = false;
        hornActive = false;
        hornPower = -35f;
        looker = FindFirstObjectByType<Looker>();
        inventory = FindFirstObjectByType<Inventory>();
        GetPlayerComponents();
        itemEnum = ItemEnum.AirHorn;
    }

    
    void Update()
    {
        if (hornActive)
        {
            animTime = armAnimBehaviour.GetAnimTime();
        }
        animMode = playerBehaviour.GetAnimMode();
        if (animMode == PlayerAnimMode.Run || animMode == PlayerAnimMode.Jump || playerBehaviour.paused)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && canHorn)
        {
            canHorn = false;
            hornUsed = true;
            armAnimBehaviour.SetTrigger("HornUse", itemAnimBehaviour);
            hornSound.Play();
        }
    }

    private void FixedUpdate()
    {
        if (hornUsed && looker != null)
        {
            hornUsed = false;
            StartCoroutine(AirHornTimer());
        }

        if (hornActive)
        {
            //Calculate relative axis
            Vector3 axis1 = Vector3.Cross(looker.LookRay().direction, looker.SideRay().direction);
            Vector3 axis2 = Vector3.Cross(looker.LookRay().direction, axis1);

            float maxDistance = 40f;
            RaycastHit[] monsters = new RaycastHit[13];
            Quaternion axisRotation1 = Quaternion.identity;
            Quaternion axisRotation2 = Quaternion.identity;


            //calculates cone of raycasts
            Physics.Raycast(looker.LookRay(), out monsters[0], maxDistance);

            float a = -10f;
            for (int i = 1; i<5; i++)
            {
                axisRotation1 = Quaternion.AngleAxis(a, axis1);
                Physics.Raycast(looker.LookRay().origin, axisRotation1 * looker.LookRay().direction, out monsters[i], maxDistance);
                a = a + 5f;
                if (a==0f)
                {
                    a = a + 5f;
                }
            }

            float b = -10f;
            for (int j = 5; j<9; j++)
            {
                axisRotation2 = Quaternion.AngleAxis(b, axis2);
                Physics.Raycast(looker.LookRay().origin, axisRotation2 * looker.LookRay().direction, out monsters[j], maxDistance);
                b = b + 5f;
                if (b == 0f)
                {
                    b = b + 5f;
                }
            }

            float c = -7.5f;
            for (int k = 9; k<11; k++)
            {
                axisRotation1 = Quaternion.AngleAxis(c, axis1);
                axisRotation2 = Quaternion.AngleAxis(c, axis2);
                Physics.Raycast(looker.LookRay().origin, axisRotation2*(axisRotation1 * looker.LookRay().direction), out monsters[k], maxDistance);

                axisRotation1 = Quaternion.AngleAxis(-c, axis1);
                axisRotation2 = Quaternion.AngleAxis(c, axis2);
                Physics.Raycast(looker.LookRay().origin, axisRotation2 * (axisRotation1 * looker.LookRay().direction), out monsters[k+2], maxDistance);

                c = c + 15f;
            }

            StartCoroutine(BlowMonsters(monsters, -looker.LookRay().direction));
        }
    }

    private IEnumerator AirHornTimer()
    {
        hornActive = true;
        armAnimBehaviour.armsLocked = true;
        yield return new WaitUntil(() => animTime >= 0.85f);
        animTime = 0f;
        armAnimBehaviour.ActivateForceSwitch();
        armAnimBehaviour.armsLocked = false;
        hornActive = false;
        inventory.RemoveItemAfterUsed(itemSlot);
    }

    private IEnumerator BlowMonsters(RaycastHit[] monsters, Vector3 blowVector)
    {
        foreach(RaycastHit monster in monsters)
        {
            if (monster.collider != null)
            {
                if (monster.transform.gameObject.layer == 16)
                { 
                    MonsterRagdoll ragdoll = monster.transform.gameObject.GetComponentInChildren<MonsterRagdoll>();
                    if (!ragdoll.RagDollActive())
                    {
                        ragdoll.StandardDeathSequence(false);
                    }
                    ragdoll.BaseShoot(blowVector * hornPower);
                }
            }
        }
        yield return null;
    }
}
