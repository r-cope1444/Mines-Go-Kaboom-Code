using System.Collections;
using UnityEngine;

//Handles ragdoll behaviour and death for monsters
public class MonsterRagdoll : RagDoll
{
    [SerializeField] Rigidbody spineRB;
    [SerializeField] GameObject orb;
    private TickleMonsterSpawner spawner;
    private GameObject monster;
    private bool resetRagdoll;

    void Start()
    {
        spawner = FindFirstObjectByType<TickleMonsterSpawner>();
        monster = GetComponentInParent<TickleMonsters>().gameObject;
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        EnableRagdoll(false, rigidBodies, colliders);
        isDead = false;
        resetRagdoll = false;
    }

    private void Update()
    {
        if (resetRagdoll)
        {
            resetRagdoll = false;
            Instantiate(orb, spineRB.gameObject.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
            Destroy(monster);
        }
    }

    private void FixedUpdate()
    {
        if (!isDead && RagDollActive())
        {
            if (spineRB.velocity.sqrMagnitude < 1)
            {
                resetRagdoll = true;
                spineRB.velocity = Vector3.zero;
            }
        }
    }

    public void StandardDeathSequence(bool dead)
    {
        isDead = dead;
        monster.GetComponent<TickleMonsters>().enabled = false;
        monster.GetComponent<Rigidbody>().isKinematic = true;
        monster.GetComponent<Collider>().enabled = false;
        EnableRagdoll(true, rigidBodies, colliders);
    }

    protected override IEnumerator Death()
    {
        if (isDead)
        {
            yield break;
        }

        StandardDeathSequence(true);

        ExplodeRagdoll(rigidBodies);

        yield return new WaitForSeconds(5f);

        bool incrementMonsterCount = false;
        spawner.DeleteMonster(incrementMonsterCount);

        Destroy(monster);
    }
    
    protected override IEnumerator Death(Vector3 ShotDirection)
    {
        if (isDead)
        {
            yield break;
        }

        StandardDeathSequence(true);

        ShootRagdoll(rigidBodies, ShotDirection);

        yield return new WaitForSeconds(5f);

        bool incrementMonsterCount = true;
        spawner.DeleteMonster(incrementMonsterCount);

        Destroy(monster);
    }

}
