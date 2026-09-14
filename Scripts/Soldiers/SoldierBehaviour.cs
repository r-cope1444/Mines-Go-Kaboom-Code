using System.Collections;
using UnityEngine;

//Soldiers that kill the player if they try to escape the beach
public class SoldierBehaviour : MonoBehaviour
{
    private float gunPower = -10f;
    [SerializeField] private GameObject soldierMesh;
    [SerializeField] private AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Debug.Log(gunPower);
            Vector3 shotDirection = gameObject.transform.position - other.gameObject.transform.position;
            Vector3 shotDirectionOnYPlane = new Vector3(shotDirection.x, 0f, shotDirection.z);
            Vector3 trajectory = shotDirectionOnYPlane.normalized * gunPower;
            Vector3 facing = transform.TransformVector(Vector3.forward);

            float angle = Vector3.SignedAngle(facing, shotDirectionOnYPlane, Vector3.up);
            soldierMesh.transform.Rotate(new Vector3(0f, angle, 0f), Space.World);
            StartCoroutine(ResetMesh());
            audioSource.Play();
            other.gameObject.GetComponentInChildren<PlayerRagdoll>().StartDeathCoroutine(trajectory);
        }
    }

    private IEnumerator ResetMesh()
    {
        yield return new WaitForSeconds(3f);
        soldierMesh.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
    }
}
