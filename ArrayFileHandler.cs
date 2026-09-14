using System;
using System.IO;
using UnityEngine;

//Reads and writes array data seperate from the rest of data
public class ArrayFileHandler
{
    private string dataDirPath = "";
    private string fileName = "";

    public ArrayFileHandler(string dataDirPath, string fileName)
    {
        this.dataDirPath = dataDirPath;
        this.fileName = fileName;
    }

    public ArrayGameData Load()
    {
        string path = Path.Combine(dataDirPath, fileName);
        ArrayGameData loadedData = null;
        if (File.Exists(path))
        {
            try
            {
                string inactiveMineData = "";
                string explodedMineData = "";
                string inactiveItemData = "";
                string invItemEnumData = "";
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        inactiveMineData = reader.ReadLine();
                        explodedMineData = reader.ReadLine();
                        inactiveItemData = reader.ReadLine();
                        invItemEnumData = reader.ReadLine();
                    }
                }

                string[] stringsToCheck = {inactiveMineData, explodedMineData, inactiveItemData, invItemEnumData };
                foreach (string str in stringsToCheck)
                {
                    if (str == null || str == "")
                    {
                        return loadedData;
                    }
                }

                loadedData = new ArrayGameData();
                loadedData.inactiveMines = JsonHelper.FromJson<bool>(inactiveMineData);
                loadedData.explodedMines = JsonHelper.FromJson<bool>(explodedMineData);
                loadedData.inactiveItems = JsonHelper.FromJson<bool>(inactiveItemData);
                loadedData.invItemEnums = JsonHelper.FromJson<ItemEnum>(invItemEnumData);
            }
            catch (Exception e)
            {
                Debug.LogError("Could not load data from:" + path + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(ArrayGameData data)
    {
        string path = Path.Combine(dataDirPath, fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            string inactiveMineData = JsonHelper.ToJson(data.inactiveMines);
            string explodedMineData = JsonHelper.ToJson(data.explodedMines);
            string inactiveItemData = JsonHelper.ToJson(data.inactiveItems);
            string invItemEnumData = JsonHelper.ToJson(data.invItemEnums);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(inactiveMineData);
                    writer.WriteLine(explodedMineData);
                    writer.WriteLine(inactiveItemData);
                    writer.WriteLine(invItemEnumData);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Could not save data to:" + path + "\n" + e);
        }
    }
}