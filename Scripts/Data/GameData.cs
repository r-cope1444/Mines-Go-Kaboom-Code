
using UnityEngine;

//General data to be loaded
[System.Serializable]
public class GameData
{
    public Vector3 playerPos;

    public float camRotX;
    public float camRotY;

    public int scene;
    public int scraps;

    public int totalItems;
    public int monstersKilled;
    public int deaths;
    public int totalScraps;
    public float timeTaken;

    public bool gameFinished;

    public GameData()
    {
        playerPos = new Vector3(143.9f, 31.67f, 265.5f);
        camRotX = 0.0f;
        camRotY = 0.0f;
        scene = 1;
        scraps = 0;
        totalItems = 0;
        monstersKilled = 0;
        deaths = 0;
        timeTaken = 0;
        totalScraps = 0;
        gameFinished = false;
    }
}
