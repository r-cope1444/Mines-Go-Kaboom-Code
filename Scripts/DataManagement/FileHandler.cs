using System;
using System.IO;
using UnityEngine;

//Reads and writes non-array data
public class FileHandler
{
    private string dataDirPath = "";
    private string fileName = "";

    public FileHandler(string dataDirPath, string fileName)
    {
        this.dataDirPath = dataDirPath;
        this.fileName = fileName;
    }

    public GameData Load()
    {
        string path = Path.Combine(dataDirPath, fileName);
        GameData loadedData = null;
        if (File.Exists(path))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Could not load data from:" + path + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string path = Path.Combine(dataDirPath, fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Could not save data to:" + path + "\n" + e);
        }
    }
}
