
using UnityEngine;

public enum PlayerAnimMode
{
    Neutral,
    Walk,
    Run,
    Jump
}

//Handles player movement
public class PlayerBehaviour : MonoBehaviour, DataInterface
{
    [SerializeField] private LayerMask grounded;
    [SerializeField] private GameObject armMeshParent;
    [SerializeField] private GameObject spine;
    [SerializeField] private GameObject playerMesh;

    [SerializeField] private Rigidbody playerMotion;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GamePause pauseScript;
    [SerializeField] private Inventory inventory;

    [Header("Audios")]
    [SerializeField] private AudioSource[] playerSounds;

    private Quaternion localCamRotation;
    private Quaternion playerMeshRotation;
    private Vector3 defaultSpinePos;
    private Quaternion defaultSpineRot;
    private Vector3 playerMeshOffset = new Vector3(0f, 1.338f, 0.26f);

    private float PI = Mathf.PI;

    public bool animLock;

    public bool paused;

    private float rotY = 0.0f;
    private float rotX = 0.0f;

    private float groundedDistance = 1.83f;
    private float maxAngle = 25f;

    public bool neutralState = true;
    private bool accelState = false;
    private bool normSpeedState = false;
    private bool maxSpeedState = false;
    private bool decelState = false;
    private bool accelState2 = false;
    private bool decelState2 = false;

    private bool wPress = false;
    private bool sPress = false;
    private bool aPress = false;
    private bool dPress = false;
    private bool sprint = false;

    private bool wState = false;
    private bool sState = false;
    private bool aState = false;
    private bool dState = false;

    private float yAngleInRads = 0;

    private float xForward = 0;
    private float zForward = 0;
    private float xBack = 0;
    private float zBack = 0;
    private float xLeft = 0;
    private float zLeft = 0;
    private float xRight = 0;
    private float zRight = 0;

    private float xFinal = 0;
    private float zFinal = 0;
    private float xInitial = 0;
    private float zInitial = 0;
    private float xVel = 0;
    private float zVel = 0;

    private bool xDecelStop = false;
    private bool zDecelStop = false;

    public float spdForward = 6f;
    public float spdBack = 3f;
    public float spdSide = 4.2f;
    public float sprintSpeed = 1.7f;

    private float diagWeight = 1f;
    private float fallingWeight = 0.5f;

    private float xMomentum = 0.0f;
    private float zMomentum = 0.0f;

    private bool playerJump = false;
    public bool jumpMode = false;
    private bool jumpPassed;
    private bool playerHasMomentum;

    private bool killInput;

    private int xSign = 1;
    private int zSign = 1;

    public float sensitivity;

    private PlayerAnimMode animMode;

    public void LoadData(GameData data)
    {
        transform.position = data.playerPos;

        rotY = data.camRotY;

        rotX = data.camRotX;

        localCamRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        mainCamera.transform.rotation = localCamRotation;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPos = transform.position;
        data.camRotX = rotX;
        data.camRotY = rotY;
    }

    void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        neutralState = true;
        defaultSpinePos = spine.transform.position;
        defaultSpineRot = spine.transform.rotation;
        animLock = false;
        paused = false;
        if (PlayerPrefs.HasKey("Look Sensitivity"))
        {
            sensitivity = PlayerPrefs.GetFloat("Look Sensitivity");
        }
        else
        {
            sensitivity = 400f;
        }
    }
    

    void Update()
    {
        paused = pauseScript.Paused();
        if (paused == true)
        {
            return;
        }

        if(animLock == true)
        {
            return ;
        }

        float mouseX = Input.GetAxis("Mouse X");

        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * sensitivity * Time.deltaTime;

        rotX += mouseY * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -80.0f, 80.0f);

        localCamRotation = Quaternion.Euler(rotX, rotY, 0.0f);

        playerMeshRotation = Quaternion.Euler(0.0f, rotY, 0.0f);

        yAngleInRads = rotY / 180f * PI;

        if (killInput == false)
        {
            wPress = ButtonPressed(KeyCode.W);
            dPress = ButtonPressed(KeyCode.D);
            aPress = ButtonPressed(KeyCode.A);
            sPress = ButtonPressed(KeyCode.S);
            sprint = ButtonPressed(KeyCode.LeftShift);
        }
        else
        {
            wPress = false;
            dPress = false;
            aPress = false;
            sPress = false;
            sprint = false;
        }

        if (neutralState == true)
        {
            animMode = PlayerAnimMode.Neutral;
            if (wPress || dPress || aPress || sPress)
            {
                neutralState = false;
                accelState = true;
                DirectionSetter();
                xFinal = (xLeft + xRight + xForward + xBack) * diagWeight;
                zFinal = (zLeft + zRight + zBack + zForward) * diagWeight;
            }
        }

        if (normSpeedState == true)
        {
            animMode = PlayerAnimMode.Walk;
            if ((wPress == false) & (dPress == false) & (aPress == false) & (sPress == false))
            {
                normSpeedState = false;
                decelState = true;
                xInitial = xVel;
                zInitial = zVel;

                if(xInitial < 0)
                {
                    xSign = -1;
                }
                else
                {
                    xSign = 1;
                }

                if(zInitial < 0)
                {
                    zSign = -1;
                }
                else
                {
                    zSign = 1;
                }
            }
            else if(sprint == true & jumpMode == false)
            {
                normSpeedState = false;
                accelState2 = true;
                DirectionSetter();
                xFinal = (xLeft + xRight + xForward + xBack) * diagWeight * sprintSpeed;
                zFinal = (zLeft + zRight + zBack + zForward) * diagWeight * sprintSpeed;
            }
            else
            {
                DirectionSetter();
                xVel = (xLeft + xRight + xForward + xBack) * diagWeight;
                zVel = (zLeft + zRight + zBack + zForward) * diagWeight;
            }
        }

        if(maxSpeedState ==  true)
        {
            animMode = PlayerAnimMode.Run;
            if ((wPress == false) & (dPress == false) & (aPress == false) & (sPress == false))
            {
                maxSpeedState = false;
                decelState = true;
                xInitial = xVel;
                zInitial = zVel;

                if (xInitial < 0)
                {
                    xSign = -1;
                }
                else
                {
                    xSign = 1;
                }

                if (zInitial < 0)
                {
                    zSign = -1;
                }
                else
                {
                    zSign = 1;
                }
            }
            else if (sprint == false)
            {
                maxSpeedState = false;
                decelState2 = true;
                DirectionSetter();
                xFinal = (xLeft + xRight + xForward + xBack) * diagWeight;
                zFinal = (zLeft + zRight + zBack + zForward) * diagWeight;
            }
            else
            {
                DirectionSetter();
                xVel = (xLeft + xRight + xForward + xBack) * diagWeight * sprintSpeed;
                zVel = (zLeft + zRight + zBack + zForward) * diagWeight * sprintSpeed;
            }
        }

        if (accelState || accelState2 || decelState || decelState2)
        {
            animMode = PlayerAnimMode.Walk;
        }

        if (jumpMode)
        {
            animMode = PlayerAnimMode.Jump;
        }

        switch (animMode)
        {
            case PlayerAnimMode.Walk:
                if (playerSounds[1].isPlaying)
                {
                    playerSounds[1].Stop();
                }
                if (!playerSounds[0].isPlaying)
                {
                    playerSounds[0].Play();
                }
                break;
            case PlayerAnimMode.Run:
                if (playerSounds[0].isPlaying)
                {
                    playerSounds[0].Stop();
                }
                if (!playerSounds[1].isPlaying)
                {
                    playerSounds[1].Play();
                }
                break;
            default:
                if (playerSounds[0].isPlaying)
                {
                    playerSounds[0].Stop();
                }
                if (playerSounds[1].isPlaying)
                {
                    playerSounds[1].Stop();
                }
                break;

        }

        if (rotY > 360f || rotY < -360f)
        {
            rotY %= 360f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerJump = true;
        }
    }

    private void LateUpdate()
    {
        mainCamera.transform.rotation = localCamRotation;
        playerMesh.transform.rotation = playerMeshRotation;
        playerMesh.transform.position = gameObject.transform.position + playerMeshOffset;
        armMeshParent.transform.rotation = localCamRotation;
        if (inventory.HasAnItem())
        {
            inventory.RotateCurrentItem(localCamRotation);
        }
    }

    private void FixedUpdate()
    {
        if (animLock == true)
        {
            return;
        }
        
        Accelerator(accelState, accelState2, decelState, decelState2);
        
        if (neutralState || maxSpeedState || normSpeedState)
        {
            RaycastHit downHit;
            Physics.Raycast(gameObject.transform.position, Vector3.down, out downHit, 10f, grounded);
            float angle = Vector3.Angle(Vector3.up, downHit.normal);

            if (downHit.distance < groundedDistance)
            {
                killInput = false;
            }

            if (jumpMode == true)
            {
                if (downHit.distance > groundedDistance)
                {
                    jumpPassed = true;
                }
            }

            if(jumpPassed == true & downHit.distance < groundedDistance)
            {
                KillMomentum();
            }

            if (playerHasMomentum == true)
            {
                if ((wState != wPress) || (sState != sPress) || (dState != dPress) || (aState != aPress))
                {
                    KillMomentum();
                }
            }

            if(downHit.distance < groundedDistance & jumpMode == false)
            {
                if (angle < maxAngle)
                {
                    Vector3 velocity = new Vector3(xVel, playerMotion.velocity.y, zVel);
                    Vector3 slopeMove = Vector3.ProjectOnPlane(velocity, downHit.normal);
                    playerMotion.velocity = slopeMove;
                }
            }
            else
            {
                playerMotion.velocity = new Vector3(xMomentum + xVel * fallingWeight, playerMotion.velocity.y, zMomentum + zVel * fallingWeight);
            }

            if (playerJump)
            {
                playerJump = false;
                if (downHit.distance < groundedDistance & angle < maxAngle)
                {
                    jumpMode = true;
                    playerMotion.AddForce(Vector3.up * 5, ForceMode.Impulse);
                    if ((playerMotion.velocity.x != 0f) || (playerMotion.velocity.z != 0f))
                    {
                        xMomentum = playerMotion.velocity.x * 0.5f;
                        zMomentum = playerMotion.velocity.z * 0.5f;
                        playerHasMomentum = true;
                        wState = wPress;
                        sState = sPress;
                        aState = aPress;
                        dState = dPress;
                    }
                }
            }
        }

        if (accelState || decelState)
        {
            playerMotion.velocity = new Vector3(xVel, playerMotion.velocity.y, zVel);
        }
    }

    private void Accelerator(bool accel1, bool accel2, bool decel1, bool decel2)
    {
        if (accel1 == true || accel2 == true)
        {
            if (mod(xVel) < mod(xFinal))
            {
                xVel = xVel + xFinal / 10;
            }

            if (mod(zVel) < mod(zFinal))
            {
                zVel = zVel + zFinal / 10;
            }

            if ((mod(xVel) >= mod(xFinal)) & (mod(zVel) >= mod(zFinal)))
            {
                if (accel1 == true)
                {
                    accelState = false;
                    normSpeedState = true;
                }
                else
                {
                    accelState2 = false;
                    maxSpeedState = true;
                }
            }
        }
        else if (decel1 == true)
        {
            if (xDecelStop == false)
            {
                if (xSign == -1)
                {
                    if (xVel < 0)
                    {
                        xVel = xVel - xInitial / 10;
                    }
                    else
                    {
                        xDecelStop = true;
                    }
                }
                else
                {
                    if (xVel > 0)
                    {
                        xVel = xVel - xInitial / 10;
                    }
                    else
                    {
                        xDecelStop = true;
                    }
                }
            }

            if (zDecelStop == false)
            {
                if (zSign == -1)
                {
                    if (zVel < 0)
                    {
                        zVel = zVel - zInitial / 10;
                    }
                    else
                    {
                        zDecelStop = true;
                    }
                }
                else
                {
                    if (zVel > 0)
                    {
                        zVel = zVel - zInitial / 10;
                    }
                    else
                    {
                        zDecelStop = true;
                    }
                }
            }

            if ((xDecelStop & zDecelStop) == true)
            {
                decelState = false;
                neutralState = true;
                xDecelStop = false;
                zDecelStop = false;
                xVel = 0;
                zVel = 0;
            }
        }
        else if (decel2 == true)
        {
            if (mod(xVel) > mod(xFinal))
            {
                xVel = xVel - xFinal / 10;
            }

            if (mod(zVel) > mod(zFinal))
            {
                zVel = zVel - zFinal / 10;
            }

            if ((mod(xVel) <= mod(xFinal)) && (mod(zVel) <= mod(zFinal)))
            {
                decelState2 = false;
                normSpeedState = true;
            }

            if((playerMotion.velocity.x==0)&&(playerMotion.velocity.z==0))
            {
                decelState2 = false;
                neutralState = true;
            }
        }
    }

    private void DirectionSetter()
    {
        if (wPress)
        {
            xForward = sin(yAngleInRads) * spdForward;
            zForward = cos(yAngleInRads) * spdForward;
        }
        else
        {
            xForward = 0;
            zForward = 0;
        }
        if (sPress)
        {
            xBack = -sin(yAngleInRads) * spdBack;
            zBack = -cos(yAngleInRads) * spdBack;
        }
        else
        {
            xBack = 0;
            zBack = 0;
        }
        if (aPress)
        {
            xLeft = sin(yAngleInRads - PI / 2) * spdSide;
            zLeft = cos(yAngleInRads - PI / 2) * spdSide;
        }
        else
        {
            xLeft = 0;
            zLeft = 0;
        }
        if (dPress)
        {
            xRight = sin(yAngleInRads + PI / 2) * spdSide;
            zRight = cos(yAngleInRads + PI / 2) * spdSide;
        }
        else
        {
            xRight = 0;
            zRight = 0;
        }

        if ((sPress || wPress) && (dPress || aPress))
        {
            diagWeight = 0.6f;
        }
        else
        {
            diagWeight = 1f;
        }

        if (wPress && sPress)
        {
            xForward = 0;
            xBack = 0;
            zForward = 0;
            zBack = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            killInput = true;
            KillMomentum();
        }
    }

    private bool ButtonPressed(KeyCode button)
    {
        return Input.GetKey(button);
    }

    private float sin(float angle)
    {
        return Mathf.Sin(angle);
    }

    private float cos(float angle)
    {
        return Mathf.Cos(angle);
    }

    private float mod(float value)
    {
        if (value < 0)
        {
            return -value;
        }
        else
        {
            return value;
        }
    }

    private void KillMomentum()
    {
        playerHasMomentum = false;
        jumpPassed = false;
        jumpMode = false;
        spine.transform.position = defaultSpinePos;
        spine.transform.rotation = defaultSpineRot;
        xMomentum = 0f;
        zMomentum = 0f;
    }

    public PlayerAnimMode GetAnimMode()
    {
        return animMode;
    }

    public void StopSounds()
    {
        if (playerSounds[0].isPlaying)
        {
            playerSounds[0].Stop();
        }
        if (playerSounds[1].isPlaying)
        {
            playerSounds[1].Stop();
        }
    }
    
}
