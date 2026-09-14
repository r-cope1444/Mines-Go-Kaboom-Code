
using UnityEngine;

//Handles game currency system
public class Money : MonoBehaviour, DataInterface
{
    [SerializeField] private PlayThroughData playThroughData;
    private int scraps = 0;

    public void LoadData(GameData data)
    {
        scraps = data.scraps;
    }

    public void SaveData(ref GameData data)
    {
        data.scraps = scraps;
    }

    public void AddScraps()
    {
        int scrapsToAdd = Random.Range(1, 6);

        scraps = scraps + scrapsToAdd;
        playThroughData.IncrementScraps(scrapsToAdd);
    }

    public bool MinusScraps(int cost)
    {
        bool canBuy = false;
        if (cost <= scraps)
        {
            canBuy = true;
            scraps = scraps - cost;
        }
        return canBuy;
    }

    public int ScrapCount()
    {
        return scraps;
    }
}
