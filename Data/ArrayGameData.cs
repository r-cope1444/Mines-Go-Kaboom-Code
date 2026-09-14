
//Arrays to be loaded
[System.Serializable]
public class ArrayGameData
{
    public bool[] inactiveMines;
    public bool[] explodedMines;
    public bool[] inactiveItems;
    public ItemEnum[] invItemEnums;

    public ArrayGameData()
    {
        inactiveMines = new bool[132];
        explodedMines = new bool[132];
        inactiveItems = new bool[26];
        invItemEnums = new ItemEnum[5];
        
    }
}
