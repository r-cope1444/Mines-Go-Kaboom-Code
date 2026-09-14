using System.Collections;
using UnityEngine;

//Handles behaviour for the monsters
public class TickleMonsters : MonoBehaviour
{
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Animator animator;

    private GameObject spawner;
    private Rigidbody monsterBody;
    private Collider monsterCollider;
    private GameObject player;


    private Vector3 monsterStartUpVel;
    private Vector3 startUpForce;

    private bool startUpDone;
    private bool findingPlayer;

    private float rotationSpeed;

    private float monsterSpeed;
    private float decimaliser;
    private float meshRotation;

    private float acceleration;


    public void Start()
    {
        animator.enabled = false;
        TickleMonsterSpawner spawnerScript = GetComponentInParent<TickleMonsterSpawner>();
        monsterBody = GetComponent<Rigidbody>();
        monsterCollider = GetComponent<Collider>();
        player = FindFirstObjectByType<PlayerBehaviour>().gameObject;
        monsterSpeed = 100f;
        decimaliser = 180f / Mathf.PI;
        if (spawnerScript == null)
        {
            startUpDone = true;
            return;
        }
        spawner = spawnerScript.gameObject;
        transform.position = transform.position + new Vector3(100f, -20f, 20f);
        startUpDone = false;
        

        float xVel = -20f;
        float yVel = 30f;
        float zVel = -4f;

        monsterStartUpVel = new Vector3(xVel, yVel, zVel);

        float horizontalSpeed = Mathf.Sqrt(xVel * xVel + zVel * zVel);
        float xDiff = spawner.transform.position.x - transform.position.x;
        float zDiff = spawner.transform.position.z - transform.position.z;
        float horizontalDistance = Mathf.Sqrt(xDiff*xDiff + zDiff*zDiff);
        float timeForStartUp = horizontalDistance / horizontalSpeed;

        float verticalSpeed = yVel;
        float verticalDistance = spawner.transform.position.y - transform.position.y;

        acceleration = 2 * (verticalDistance - verticalSpeed*timeForStartUp)/(timeForStartUp*timeForStartUp);

        startUpForce = new Vector3(0f, acceleration, 0f);

        monsterBody.velocity = monsterStartUpVel;
        rotationSpeed = 360f/ timeForStartUp;

        StartCoroutine(StartUpTimer(timeForStartUp));
    }

    private void FixedUpdate()
    {
        if (!startUpDone)
        {
            monsterBody.AddForce(startUpForce, ForceMode.Acceleration);
            Quaternion deltaRotation = Quaternion.Euler(0f, rotationSpeed * Time.fixedDeltaTime, 0f);
            monsterBody.MoveRotation(monsterBody.rotation * deltaRotation);
            return;
        }

        if (player == null)
        {
            if (!findingPlayer)
            {
                animator.SetBool("Walking", false);
                monsterBody.velocity = new Vector3(0, monsterBody.velocity.y, 0);
                StartCoroutine(WaitForPlayer());
                findingPlayer = true;
            }
            return;
        }

        if (player.GetComponent<PlayerBehaviour>().enabled == false)
        {
            animator.SetBool("Walking", false);
            monsterBody.velocity = new Vector3(0, monsterBody.velocity.y, 0);
            return;
        }

        RaycastHit hitGround;
        bool onGround = Physics.Raycast(transform.position, Vector3.down, out hitGround, 1.5f, ground);
        float angle = Vector3.Angle(Vector3.up, hitGround.normal);



        if (!onGround)
        {
            return;
        }

        Vector3 distanceToPlayer = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
        Vector3 normalisedVector = distanceToPlayer / distanceToPlayer.magnitude;

        if (angle<85f)
        {
            animator.SetBool("Walking", true);
            Vector3 slopeMove = Vector3.ProjectOnPlane(normalisedVector, hitGround.normal);
            monsterBody.velocity = slopeMove * monsterSpeed * Time.fixedDeltaTime;

            meshRotation = RotationCalc(normalisedVector.x, normalisedVector.z);

            transform.rotation = Quaternion.Euler(0.0f, meshRotation, 0.0f);
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        RaycastHit hitPlayer;
        bool tickledPlayer = Physics.Raycast(transform.position, normalisedVector, out hitPlayer, 2f, playerMask);
        if (tickledPlayer)
        {
            hitPlayer.transform.gameObject.GetComponentInChildren<PlayerRagdoll>().StartDeathCoroutine(normalisedVector);
        }
    }

    private float RotationCalc(float z, float x)
    {
        float rot = 0f;

        if (x==0)
        {
            if (z>0)
            {
                rot = 90f;
            }
            else
            {
                rot = 270f;
            }
        }
        else if(x>0)
        {
            if (z>=0)
            {
                rot = Mathf.Atan(z/x) * decimaliser;
            }
            else
            {
                rot = 360f - (Mathf.Atan(z/x) * -decimaliser);
            }
        }
        else
        {
            if (z>0)
            {
                rot = 180f - (Mathf.Atan(z / x) * -decimaliser);
            }
            else
            {
                rot = 180f + Mathf.Atan(z / x) * decimaliser;
            }
        }

        return rot;
    }

    private IEnumerator WaitForPlayer()
    {
        yield return new WaitWhile(() => FindFirstObjectByType<PlayerBehaviour>() == null);
        player = FindFirstObjectByType<PlayerBehaviour>().gameObject;
        findingPlayer = false;
    }

    private IEnumerator StartUpTimer(float time)
    {
        yield return new WaitForSeconds(time);
        SetDefaultPhysicsProperties();
    }

    public void SetDefaultPhysicsProperties()
    {
        animator.enabled = true;
        monsterBody.rotation = Quaternion.identity;
        monsterBody.useGravity = true;
        monsterCollider.enabled = true;
        startUpDone = true;
        monsterBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        monsterBody.velocity = Vector3.zero;
    }


    private void OnDisable()
    {
        animator.SetBool("Walking", false);
    }
}
