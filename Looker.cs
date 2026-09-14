
using UnityEngine;

//Creates rays for player interactions
public class Looker : MonoBehaviour
{
    public GameObject mainCamera;

    private Ray ray;
    private Ray sideRay;

    
    void Update()
    {
        Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0f);
        Vector3 screenSide = new Vector3(0f, 0.5f, 0f);
        ray = mainCamera.GetComponent<Camera>().ViewportPointToRay(screenCenter);
        sideRay = mainCamera.GetComponent<Camera>().ViewportPointToRay(screenSide);
    }

    public Ray LookRay()
    {
        return ray;
    }

    public Ray SideRay()
    {
        return sideRay;
    }
}
