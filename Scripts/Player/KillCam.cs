
using UnityEngine;

//Camera for when the player is killed
public class KillCam : MonoBehaviour
{
    private GameObject player;
    private Vector3 distanceRelToRagdoll;

    public void Start()
    {
        FindNewPlayer();
        distanceRelToRagdoll = new Vector3(0f, 6f, -4f);
    }

    public void Update()
    {
        if (gameObject.GetComponent<Camera>().enabled == true)
        {
            if(player == null)
            {
                return;
            }
            Vector3 ragdollCamPos = distanceRelToRagdoll + player.transform.position;
            gameObject.transform.position = ragdollCamPos;
        }
    }

    public void FindNewPlayer()
    {
        player = FindFirstObjectByType<PlayerBehaviour>().gameObject;
    }
}
