using System.Collections;
using UnityEngine;

//Parent class for player and monster ragdolls.
public class RagDoll : MonoBehaviour
{
    protected bool isDead;
    protected bool ragDollActive;

    protected Rigidbody[] rigidBodies;
    protected Collider[] colliders;



    protected void EnableRagdoll(bool state, Rigidbody[] bodies, Collider[] colliders)
    {
        foreach (Rigidbody rb in bodies)
        {
            rb.isKinematic = !state;
        }
        foreach (Collider col in colliders)
        {
            col.enabled = state;
        }
        GetComponent<Animator>().enabled = !state;
        ragDollActive = state;
    }

    protected void ExplodeRagdoll(Rigidbody[] bodies)
    {
        float x = Random.Range(-5f, 5f);
        float z = Random.Range(-5f, 5f);


        Vector3 explodeDirection = new Vector3(x, 20f, z);

        foreach (Rigidbody rb in bodies)
        {
            rb.AddForce(explodeDirection, ForceMode.Impulse);
        }
    }

    protected void ShootRagdoll(Rigidbody[] bodies, Vector3 KnockBack)
    {
        foreach (Rigidbody rb in bodies)
        {
            rb.AddForce(KnockBack, ForceMode.Impulse);
        }
    }



    public bool IsDead()
    {
        return isDead;
    }

    public bool RagDollActive()
    {
        return ragDollActive;
    }



    //Public methods to access all coroutines since in some cases the objects calling them are destroyed, and the coroutines rely on
    // the MonoBehaviour.StartCoroutine() still existing to run.
    public void StartDeathCoroutine()
    {
        StartCoroutine(Death());
    }

    public void StartDeathCoroutine(Vector3 shotDirection)
    {
        StartCoroutine(Death(shotDirection));
    }

    public void BaseExplode()
    {
        StartCoroutine(ExplodeCoroutine());
    }

    public void BaseShoot(Vector3 shotDirection)
    {
        StartCoroutine(Shoot(shotDirection));
    }



    protected IEnumerator ExplodeCoroutine()
    {
        ExplodeRagdoll(rigidBodies);
        yield return null;
    }

    protected IEnumerator Shoot(Vector3 shotDirection)
    {
        ShootRagdoll(rigidBodies, shotDirection);
        yield return null;
    }

    //The death functions are always overidden, and are virtual only to allow other methods
    // in the class to be called directly (to avoid being abstract).
    virtual protected IEnumerator Death()
    {
        if (isDead)
        {
            yield break;
        }

        yield return new WaitForSeconds(5f);

        Destroy(gameObject);
    }

    virtual protected IEnumerator Death(Vector3 ShotDirection)
    {
        if (isDead)
        {
            yield break;
        }

        yield return new WaitForSeconds(5f);

        Destroy(gameObject);
    }
}
