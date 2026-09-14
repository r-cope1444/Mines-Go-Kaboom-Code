
using UnityEngine;

//Stats to be displayed in the end scene
public class PlayThroughData : MonoBehaviour, DataInterface
{
    private int totalItems = 0;
    private int monstersKilled = 0;
    private int deaths = 0;
    private int totalScraps = 0;
    private float timeTaken = 0f;

    public void LoadData(GameData data)
    {
        totalItems = data.totalItems;
        monstersKilled = data.monstersKilled;
        deaths = data.deaths;
        timeTaken = data.timeTaken;
        totalScraps = data.totalScraps;
    }

    public void SaveData(ref GameData data)
    {
        data.totalItems = totalItems;
        data.monstersKilled = monstersKilled;
        data.deaths = deaths;
        data.timeTaken = timeTaken + Time.timeSinceLevelLoad;
        data.totalScraps = totalScraps;
    }

    public void IncrementItems()
    {
        totalItems++;
    }

    public void IncrementMonsters()
    {
        monstersKilled++;
    }

    public void IncrementDeaths()
    {
        deaths++;
    }

    public void IncrementScraps(int scraps)
    {
        totalScraps = totalScraps + scraps;
    }

}
