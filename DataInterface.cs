//Interfaces for saving and loading data for different classes

public interface DataInterface
{
    void LoadData(GameData data);

    void SaveData(ref GameData data);
}

public interface ArrayDataInterface
{
    void LoadData(ArrayGameData arrayGameData);

    void SaveData(ref ArrayGameData arrayGameData);
}
