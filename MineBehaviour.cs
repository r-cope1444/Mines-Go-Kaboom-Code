using UnityEngine;

//Lets individual mines blow up the player and monsters.
public class MineBehaviour : MonoBehaviour
{
    [SerializeField] LayerMask entityMask;
    [SerializeField] MeshRenderer mineMesh;
    [SerializeField] Material deactiveMat;
    [SerializeField] AudioSource explosionSound;
    [SerializeField] Collider mineCollider;

    private MineCounter mineCounter;
    private int mineID;

    public TerrainData TerrainData;

    private void Start()
    {
        mineCounter = GetComponentInParent<MineCounter>();
    }
    

    public void Explode()
    {
        explosionSound.Play();
        ColliderEnable(false);
        Vector3 position = transform.position;
        TerrainDigger.Dig(position.x, position.z, TerrainData, 8, 8, 0.6f);
        Collider[] entities = Physics.OverlapSphere(position, 2f, entityMask);
        foreach (Collider entity in entities)
        {
            RagDoll ragDoll = entity.gameObject.GetComponentInChildren<RagDoll>();
            if (ragDoll.IsDead() == false)
            {
                ragDoll.StartDeathCoroutine();
            }
            else if (ragDoll.RagDollActive() == true)
            {
                ragDoll.BaseExplode();
            }
        }
        mineCounter.ExplodeAMine(mineID);
        SetMesh(false);
        enabled = false;
    }
    
    public void Disarm()
    {
        ColliderEnable(false);
        mineCounter.DisarmAMine(mineID);
        LightOff(true);
        enabled = false;
    }

    public void SetMineID(int ID)
    {
        mineID = ID;
    }

    public void SetMesh(bool meshState)
    {
        mineMesh.enabled = meshState;
    }

    public void LightOff(bool state)
    {
        if (state)
        {
            mineMesh.material = deactiveMat;
        }
    }

    public void ColliderEnable(bool state)
    {
        mineCollider.enabled = state;
    }
}
