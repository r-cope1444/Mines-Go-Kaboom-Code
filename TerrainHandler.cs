using System;
using System.Globalization;
using System.IO;
using UnityEngine;

//Saves and loads the terrain heights
public class TerrainHandler
{
    private string dataPath;
    private string fileName;

    public TerrainHandler(string dataPath, string fileName)
    {
        this.dataPath = dataPath;
        this.fileName = fileName;
    }

    public void SaveTerrain(TerrainData terrainData)
    {
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        string heightDataPath = Path.Combine(dataPath, fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(heightDataPath));
            using (StreamWriter writer = new StreamWriter(heightDataPath))
            {
                int rows = heightMap.GetLength(0);
                int cols = heightMap.GetLength(1);

                for (int i = 0; i < rows; i++)
                {
                    string[] rowValues = new string[cols];
                    for (int j = 0; j < cols; j++)
                    {
                        rowValues[j] = heightMap[i, j].ToString();
                    }

                    writer.WriteLine(string.Join(" ", rowValues));
                }
            }
        }
        catch (Exception d)
        {
            Debug.LogError("Could not save terrain height map to:" + heightDataPath + "\n" + d);
        }
    }

    public float[,] LoadTerrain(TerrainData terrainData)
    {
        string heightDataPath = Path.Combine(dataPath, fileName);
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
        if (File.Exists(heightDataPath))
        {
            try
            {
                using (StreamReader reader = new StreamReader(heightDataPath))
                {
                    int rows = heightMap.GetLength(0);
                    int cols = heightMap.GetLength(1);

                    for (int i = 0; i < rows; i++)
                    {
                        string currentRow = reader.ReadLine();
                        string[] rowValues = currentRow.Split(" ");
                        for (int j = 0; j < cols; j++)
                        {
                            heightMap[i, j] = float.Parse(rowValues[j], CultureInfo.InvariantCulture.NumberFormat);
                        }
                    }
                }
            }
            catch (Exception d)
            {
                Debug.LogError("Could not load terrain height map from:" + heightDataPath + "\n" + d);
            }
        }
        return heightMap;
    }
}
