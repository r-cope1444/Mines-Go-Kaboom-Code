using System.Collections;
using UnityEngine;

//Shooting behaviour for the revolver. Can only shoot monsters
public class Shooter : Item
{
    [SerializeField] private ItemAnimBehaviour itemAnimBehaviour;
    [SerializeField] private AudioSource gunSound;

    private bool canShoot;
    private bool shotFired;
    private bool gunUsed;
    private bool gunInAnim;

    private Looker looker;
    private Inventory inventory;

    private float gunPower;
    private float animTime;

    void Start()
    {
        canShoot = true;
        shotFired = false;
        gunUsed = false;
        looker = FindFirstObjectByType<Looker>();
        inventory = FindFirstObjectByType<Inventory>();
        gunPower = -15f;
        GetPlayerComponents();
        itemEnum = ItemEnum.Gun;
    }


    void Update()
    {
        if (gunInAnim)
        {
            animTime = armAnimBehaviour.GetAnimTime();
        }
        animMode = playerBehaviour.GetAnimMode();
        if (animMode == PlayerAnimMode.Run || animMode == PlayerAnimMode.Jump || playerBehaviour.paused)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            canShoot = false;
            shotFired = true;
        }
        if (gunUsed)
        {
            inventory.RemoveItemAfterUsed(itemSlot);
        }
    }

    private void FixedUpdate()
    {
        if (shotFired && looker != null)
        {
            shotFired = false;
            RaycastHit hit;
            Physics.Raycast(looker.LookRay(), out hit, 200f);

            if (hit.collider.gameObject.layer == 10)
            {
                canShoot = true;
                return;
            }
            gunInAnim = true;
            armAnimBehaviour.SetTrigger("GunShoot", itemAnimBehaviour);
            gunSound.Play();
            StartCoroutine(ShootTimer());
            if (hit.collider == null)
            {
                return;
            }
            if (hit.transform.gameObject.layer == 16)
            {
                Vector3 shotDirection = gameObject.transform.position - hit.transform.position;
                Vector3 trajectory = shotDirection.normalized * gunPower;
                hit.transform.gameObject.GetComponentInChildren<MonsterRagdoll>().StartDeathCoroutine(trajectory);
            }
        }
    }

    private IEnumerator ShootTimer()
    {
        armAnimBehaviour.armsLocked = true;
        yield return new WaitUntil(() => animTime >= 0.85f);
        gunInAnim = false;
        animTime = 0f;
        armAnimBehaviour.ActivateForceSwitch();
        armAnimBehaviour.armsLocked = false;
        gunUsed = true;
    }
}
