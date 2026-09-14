using System.Collections;
using UnityEngine;
using System;

//Detects mines for the player and monsters
public class MineDetector : MonoBehaviour
{
    [SerializeField] private LayerMask minesOnly;

    [SerializeField] private RagDoll ragDoll;
    [SerializeField] private GameObject spine;

    private void FixedUpdate()
    {
        if (ragDoll == null)
        {
            return;
        }

        Collider[] mines = Physics.OverlapBox(ragDoll.gameObject.transform.position, new Vector3(0.6f, 1.9f, 0.6f), spine.transform.rotation, minesOnly, QueryTriggerInteraction.Collide);

        if (mines.Length != 0)
        {
            StartCoroutine(MineActivation(mines));
        }
    }

    private IEnumerator MineActivation(Collider[] minesHit)
    {
        foreach (Collider mine in minesHit)
        {
            try
            {
                MineBehaviour mineBehaviour = mine.gameObject.GetComponent<MineBehaviour>();
                if (mineBehaviour.enabled)
                {
                    mineBehaviour.Explode();
                }
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e.ToString());
            }
        }
        yield return null;
    }
}
