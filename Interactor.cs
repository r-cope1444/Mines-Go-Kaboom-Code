
using UnityEngine;
using System.Collections;
using TMPro;

//A basic class that raycasts on user input to either disarm a mine or pick up/buy an item. 
public class Interactor : MonoBehaviour
{
    [SerializeField] LayerMask interactables;
    [SerializeField] private GameObject eToInteract;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private ArmAnimBehaviour armAnimBehaviour;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private AudioSource disarmSound;
    [SerializeField] private TextMeshProUGUI notClearedMines;

    private Money money;
    private GameObject hitObject;

    public Inventory inventory;

    public Looker looker;

    private bool interact;

    private bool disarmActive;
    private float animTime;

    private int layer;
    private string label;
    private bool anyHit;
    
    void Start()
    {
        interact = false;
        money = FindFirstObjectByType<Money>();
    }

    private void OnEnable()
    {
        anyHit = false;
        interact = false;
        interactText.text = " ";
        eToInteract.SetActive(false);
    }

    void Update()
    {
        if (disarmActive)
        {
            animTime = armAnimBehaviour.GetAnimTime();
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            interact = true;
        }
        else
        {
            interact = false;
        }

        if (playerBehaviour.GetAnimMode() == PlayerAnimMode.Jump || playerBehaviour.GetAnimMode() == PlayerAnimMode.Run || playerBehaviour.paused)
        {
            return;
        }

        switch (layer)
        {
            case 9:
                eToInteract.SetActive(true);
                interactText.text = "Disarm mine";
                if (interact)
                {
                    interact = false;
                    MineBehaviour mine = hitObject.GetComponent<MineBehaviour>();
                    if (mine.enabled && !disarmActive)
                    {
                        StartCoroutine(DisarmTimer(mine));
                    }
                }
                break;

            case 15:
                eToInteract.SetActive(true);
                interactText.text = $"Pick up {label}";
                if (interact)
                {
                    interact = false;
                    StartCoroutine(inventory.AddItem(label));
                    hitObject.GetComponent<PickupItem>().DisableItem();
                }

                break;

            case 17:
                eToInteract.SetActive(true);
                int price = 0;
                switch (label)
                {
                    case "AirHorn":
                        price = 3;
                        break;

                    case "Gun":
                        price = 5;
                        break;

                    case "Vicodin":
                        price = 1;
                        break;

                    case "Shovel":
                        price = 0;
                        break;
                }
                if (price == 0)
                {
                    interactText.text = $"Take a free {label}";
                }
                else
                {
                    interactText.text = $"Buy {label} for {price} scraps";
                }

                if (interact)
                {
                    interact = false;
                    if (money.MinusScraps(price))
                    {
                        StartCoroutine(inventory.AddItem(label));
                    }
                }
                break;
            case 20:
                eToInteract.SetActive(true);
                interactText.text = "Submit beach for inspection";
                if (interact)
                {
                    interact = false;
                    hitObject.GetComponent<EndSoldier>().CheckForMines(notClearedMines);
                }
                break;
            default:
                interactText.text = " ";
                eToInteract.SetActive(false);
                break;

        }
    }

    private void FixedUpdate()
    {
        RaycastHit hitInteractable;
        anyHit = Physics.Raycast(looker.LookRay(), out hitInteractable, 5f, interactables, QueryTriggerInteraction.Collide);

        if (anyHit)
        {
            hitObject = hitInteractable.collider.gameObject;
            layer = hitInteractable.collider.gameObject.layer;
            label = hitInteractable.collider.gameObject.tag;
        }
        else
        {
            layer = 0;
        }
    }

    private IEnumerator DisarmTimer(MineBehaviour mine)
    {
        armAnimBehaviour.SetTrigger("Disarm", null);
        disarmSound.Play();
        playerBehaviour.animLock = true;
        armAnimBehaviour.armsLocked = true;
        disarmActive = true;
        yield return new WaitUntil(() => animTime >= 0.9f);
        animTime = 0f;
        mine.Disarm();
        money.AddScraps();
        disarmActive = false;
        armAnimBehaviour.ActivateForceSwitch();
        playerBehaviour.animLock = false;
        armAnimBehaviour.armsLocked = false;
    }

    public void OnDisable()
    {
        interactText.text = " ";
        eToInteract.SetActive(false);
    } 
}
