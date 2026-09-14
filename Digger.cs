using System.Collections;
using UnityEngine;

//Controls the shovel behaviour.
public class Digger : Item
{
    [SerializeField] private ItemAnimBehaviour itemAnimBehaviour;
    [SerializeField] private AudioSource digSound;

    public TerrainData terrainData;

    private Looker looker;

    private bool digHole;
    private bool canDig;
    private bool digging;

    private float animTime;

    private void Start()
    {
        canDig = true;
        digHole = false;
        looker = gameObject.GetComponentInParent<Looker>();
        GetPlayerComponents();
        itemEnum = ItemEnum.Shovel;
    }


    void Update()
    {
        if (digging)
        {
            animTime = armAnimBehaviour.GetAnimTime();
        }
        animMode = playerBehaviour.GetAnimMode();
        if (animMode != PlayerAnimMode.Neutral || playerBehaviour.paused)
        {
            return;
        }
        if (canDig == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                digHole = true;
                canDig = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (digHole == true)
        {
            digHole = false;

            if (playerBehaviour.neutralState == false || playerBehaviour.jumpMode == true)
            {
                canDig = true;
                return;
            }

            RaycastHit hit;
            Physics.Raycast(looker.LookRay(), out hit, 5f);

            if (hit.collider == null)
            {
                canDig = true;
                return;
            }
            if (hit.transform.gameObject.layer != 8)
            {
                canDig = true;
                return;
            }

            StartCoroutine(Dig(hit.point.x, hit.point.z));
        }
    }

    private IEnumerator Dig(float xTarget, float zTarget)
    {
        armAnimBehaviour.SetTrigger("ShovelDig", itemAnimBehaviour);
        digSound.Play();
        playerBehaviour.animLock = true;
        armAnimBehaviour.armsLocked = true;
        digging = true;
        yield return new WaitUntil(() => animTime >= 0.9f);
        animTime = 0f;
        TerrainDigger.Dig(xTarget, zTarget, terrainData, 6, 6, 0.6f);
        digging = false;
        canDig = true;
        armAnimBehaviour.ActivateForceSwitch();
        playerBehaviour.animLock = false;
        armAnimBehaviour.armsLocked = false;
    }
}
