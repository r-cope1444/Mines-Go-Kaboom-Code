using System.Collections;
using UnityEngine;

//Handles ragdoll behaviour and death for the player
public class PlayerRagdoll : RagDoll
{
    [SerializeField] Camera mainCam;
    [SerializeField] Camera fpCam;
    [SerializeField] GamePause gamePause;

    private bool drugged;

    private Respawn respawn;
    private GameObject killCam;
    private GameObject player;
    private PlayThroughData playThroughData;

    void Start()
    {
        player = FindFirstObjectByType<PlayerBehaviour>().gameObject;
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        respawn = FindFirstObjectByType<Respawn>();
        killCam = FindFirstObjectByType<KillCam>().gameObject;
        playThroughData = FindFirstObjectByType<PlayThroughData>();
        EnableRagdoll(false, rigidBodies, colliders);
        isDead = false;
        drugged = false;
    }

    public void StandardDeathSequence(bool dead)
    {
        gamePause.enabled = false;
        isDead = dead;
        playThroughData.IncrementDeaths();
        player.GetComponent<PlayerBehaviour>().StopSounds();
        player.GetComponent<PlayerBehaviour>().enabled = false;
        player.GetComponentInChildren<Inventory>().gameObject.SetActive(false);
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Collider>().enabled = false;
        mainCam.enabled = false;
        fpCam.enabled = false;
        player.GetComponentInChildren<AudioListener>().enabled = false;
        killCam.GetComponent<Camera>().enabled = true;
        killCam.GetComponent<AudioListener>().enabled = true;
        EnableRagdoll(true, rigidBodies, colliders);
    }

    public void StartDruggedCoroutine()
    {
        StartCoroutine(Drugged());
    }

    protected override IEnumerator Death()
    {
        if (isDead)
        {
            yield break;
        }
        if (drugged)
        {
            yield break;
        }

        StandardDeathSequence(true);

        ExplodeRagdoll(rigidBodies);

        yield return new WaitForSeconds(5f);

        respawn.RespawnProcess();
        Destroy(player);
    }

    protected override IEnumerator Death(Vector3 ShotDirection)
    {
        if (isDead)
        {
            yield break;
        }
        if (drugged)
        {
            yield break;
        }

        StandardDeathSequence(true);

        ShootRagdoll(rigidBodies, ShotDirection);

        yield return new WaitForSeconds(5f);

        respawn.RespawnProcess();
        Destroy(player);
    }

    private IEnumerator Drugged()
    {
        drugged = true;
        yield return new WaitForSeconds(5f);
        drugged = false;
    }



}
