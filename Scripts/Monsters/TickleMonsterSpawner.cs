using System.Collections;
using UnityEngine;

//Randomly spawns the monsters
public class TickleMonsterSpawner : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private GameObject tickleMonster;

    private PlayThroughData playThroughData;

    private Vector3 boxCentre;
    private Vector3 boxExtents;

    private bool startCoroutine;
    private bool coroutineRunning;

    private int tickleMonsterCount;
    
    void Start()
    {
        boxCentre = transform.position;
        boxExtents = new Vector3(200f, 100f, 200f);
        startCoroutine = false;
        coroutineRunning = false;
        playThroughData = FindFirstObjectByType<PlayThroughData>();
    }
    
    void Update()
    {
        if (startCoroutine && !coroutineRunning)
        {
            StartCoroutine(SpawnTickleMonster());
            startCoroutine = false;
            coroutineRunning = true;
        }
    }

    private void FixedUpdate()
    {
        Collider[] player = Physics.OverlapBox(boxCentre, boxExtents, transform.rotation, playerMask);

        if (player.Length == 0)
        {
            return;
        }

        if (!coroutineRunning)
        {
            startCoroutine = true;
        }
    }

    public void DeleteMonster(bool increment)
    {
        tickleMonsterCount--;
        if (increment)
        {
            playThroughData.IncrementMonsters();
        }
    }

    private IEnumerator SpawnTickleMonster()
    {
        yield return new WaitForSeconds(30f);
        float chance = Random.Range(1f, 10f);
        if (chance < 4f && tickleMonsterCount < 3)
        {
            Instantiate(tickleMonster, gameObject.transform);
            tickleMonsterCount++;
        }
        coroutineRunning = false;
    }
}
