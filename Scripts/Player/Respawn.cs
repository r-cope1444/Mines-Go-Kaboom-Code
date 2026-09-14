
using UnityEngine;

//Respawn script for player
public class Respawn : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private GameObject killCam;

    private bool respawnProcess;

    public void Start()
    {
        killCam = FindFirstObjectByType<KillCam>().gameObject;
        respawnProcess = false;
    }

    public void Update()
    {
        if (respawnProcess)
        {
            if (FindFirstObjectByType<PlayerBehaviour>() == null)
            {
                killCam.GetComponent<Camera>().enabled = false;
                killCam.GetComponent<AudioListener>().enabled = false;
                Instantiate(playerPrefab, gameObject.transform.position, Quaternion.identity);
                killCam.GetComponent<KillCam>().FindNewPlayer();
                respawnProcess =false;
            }
        }
    }

    public void RespawnProcess()
    {
        respawnProcess = true;
    }
}
