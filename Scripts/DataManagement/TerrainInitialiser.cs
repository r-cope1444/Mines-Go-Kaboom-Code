using System;
using System.IO;
using UnityEngine;
using System.Globalization;

//Initialises the heightmap for the terrain and loads the original data for when a new game is created
public class TerrainInitialiser
{
    private string dataPath;
    private string fileName;
    private TerrainData terrainData;

    public TerrainInitialiser(string dataPath, string fileName, TerrainData terrainData)
    {
        this.dataPath = dataPath;
        this.fileName = fileName;
        this.terrainData = terrainData;
    }

    public void InitialiseTerrain()
    {
        Debug.Log("Initialise Terrain");
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        string heightDataPath = Path.Combine(dataPath, fileName);
        if (File.Exists(heightDataPath))
        {
            return;
        }
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

                Debug.Log(rows);
                Debug.Log(cols);
            }
        }
        catch (Exception d)
        {
            Debug.LogError("Could not initialise default terrain height map to:" + heightDataPath + "\n" + d);
        }
    }

    public float[,] LoadTerrain()
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
                Debug.Log(heightMap.GetLength(0));
                Debug.Log(heightMap.GetLength(1));
                Debug.LogError("Could not load default terrain height map from:" + heightDataPath + "\n" + d);
            }
        }
        Debug.Log(heightMap.GetLength(0));
        Debug.Log(heightMap.GetLength(1));
        return heightMap;
    }
}
