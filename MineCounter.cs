using UnityEngine;

//Determines wether mines in scene are active, disarmed or have exploded.
public class MineCounter : MonoBehaviour, ArrayDataInterface
{
    [SerializeField] private MineBehaviour[] mineArray;

    private bool[] inactiveMines;
    private bool[] explodedMines;
    private int activeMines = 0;

    public void LoadData(ArrayGameData data)
    {
        inactiveMines = data.inactiveMines;
        explodedMines = data.explodedMines;
        int ID = 0;
        foreach (MineBehaviour mine in mineArray)
        {
            mine.SetMineID(ID);
            mine.SetMesh(!explodedMines[ID]);
            mine.enabled = !inactiveMines[ID];
            mine.ColliderEnable(!inactiveMines[ID]);
            if (!inactiveMines[ID])
            {
                activeMines++;
            }
            if (!explodedMines[ID])
            {
                mine.LightOff(inactiveMines[ID]);
            }
            ID++;
        }
    }

    public void SaveData(ref ArrayGameData data)
    {
        data.inactiveMines = inactiveMines;
        data.explodedMines = explodedMines;
    }

    public int GetMineCount()
    {
        return activeMines;
    }

    public void DecrementMineCount()
    {
        activeMines--;
    }

    public void DisarmAMine(int ID)
    {
        inactiveMines[ID] = true;
        DecrementMineCount();
    }

    public void ExplodeAMine(int ID)
    {
        inactiveMines[ID] = true;
        explodedMines[ID] = true;
        DecrementMineCount();
    }
}
