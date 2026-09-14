
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Manager for the data to be saved and loaded in the game and end scenes
public class DataManager : MonoBehaviour
{
    [Header("File Storage Config")]

    [SerializeField] private string mainDataFileName;
    [SerializeField] private string arrayDataFileName;
    [SerializeField] private string defaultMapFileName;
    [SerializeField] private string heightMapFileName;
    

    private GameData gameData;
    private ArrayGameData arrayGameData;

    public TerrainData terrainData;

    private List<DataInterface> dataPersistentObjects;
    private List<ArrayDataInterface> arrayPersistentObjects;

    private FileHandler dataHandler;

    private ArrayFileHandler arrayFileHandler;

    private TerrainInitialiser terrainInitialiser;

    private TerrainHandler terrainHandler;

    public static DataManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple Data Managers");
        }
        instance = this;
    }

    private void Start()
    {
        dataHandler = new FileHandler(Application.persistentDataPath, mainDataFileName);
        arrayFileHandler = new ArrayFileHandler(Application.persistentDataPath, arrayDataFileName);
        if (terrainData != null)
        {
            terrainInitialiser = new TerrainInitialiser(Application.persistentDataPath, defaultMapFileName, terrainData);
            terrainHandler = new TerrainHandler(Application.persistentDataPath, heightMapFileName);
        }
        dataPersistentObjects = FindDataPersistentObjs();
        arrayPersistentObjects = FindArrayPersistentObjs();
        LoadGame();
    }

    //starts new game by initialising default game state
    public void NewGame()
    {
        gameData = new GameData();
        arrayGameData = new ArrayGameData();
        if (terrainData != null)
        {
            terrainData.SetHeights(0, 0, terrainInitialiser.LoadTerrain());
            terrainHandler.SaveTerrain(terrainData);
        }
        Debug.Log(arrayGameData.inactiveMines[0]);
        dataHandler.Save(gameData);
        arrayFileHandler.Save(arrayGameData);
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();
        arrayGameData = arrayFileHandler.Load();
        if (gameData == null || arrayGameData == null)
        {
            Debug.Log("Initialising");
            if (terrainData != null)
            {
                terrainInitialiser.InitialiseTerrain();
            }
            NewGame();
        }

        if (terrainData != null)
        {
            terrainData.SetHeights(0, 0, terrainHandler.LoadTerrain(terrainData));
        }

        foreach (DataInterface persistentObject in dataPersistentObjects)
        {
            persistentObject.LoadData(gameData);
        }

        foreach (ArrayDataInterface arrayPersistentObject in arrayPersistentObjects)
        {
            arrayPersistentObject.LoadData(arrayGameData);
        }
    }

    public void SaveGame()
    {
        dataPersistentObjects = FindDataPersistentObjs();
        arrayPersistentObjects = FindArrayPersistentObjs();

        foreach (DataInterface persistentObject in dataPersistentObjects)
        {
            persistentObject.SaveData(ref gameData);
        }

        foreach (ArrayDataInterface arrayPersistentObject in arrayPersistentObjects)
        {
            arrayPersistentObject.SaveData(ref arrayGameData);
        }

        if (terrainData != null)
        {
            terrainHandler.SaveTerrain(terrainData);
        }
        dataHandler.Save(gameData);
        arrayFileHandler.Save(arrayGameData);
    }

    private List<DataInterface> FindDataPersistentObjs()
    {
        IEnumerable<DataInterface> dataPersistentObjects = FindObjectsOfType<MonoBehaviour>().OfType<DataInterface>();

        return new List<DataInterface>(dataPersistentObjects);
    }

    private List<ArrayDataInterface> FindArrayPersistentObjs()
    {
        IEnumerable<ArrayDataInterface> arrayPersistentObjects = FindObjectsOfType<MonoBehaviour>().OfType<ArrayDataInterface>();

        return new List<ArrayDataInterface>(arrayPersistentObjects);
    }

    private void OnApplicationQuit()
    {
        //SaveGame();
    }
}
