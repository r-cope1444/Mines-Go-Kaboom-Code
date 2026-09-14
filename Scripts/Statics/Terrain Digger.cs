
using UnityEngine;

//Digs holes for mines and shovels
public static class TerrainDigger
{
    public static void Dig(float xCentCoor, float zCentCoor, TerrainData terrainData, int xSize, int zSize, float holeDepth)
    {
        int xCentre = (int)Mathf.Round(xCentCoor / terrainData.heightmapScale.x);
        int zCentre = (int)Mathf.Round(zCentCoor / terrainData.heightmapScale.z);

        int xTranslate = (int)(xSize/2f);
        int zTranslate = (int)(zSize/2f);

        float[,] oldHeights = terrainData.GetHeights(xCentre - xTranslate, zCentre - zTranslate, xSize, zSize);
        float[,] newHeights = new float[zSize, xSize];

        for (int j = 0; j < zSize; j++)
            for (int i = 0; i < xSize; i++)
                if ((oldHeights[j, i] >= (21f / terrainData.heightmapScale.y)) & (oldHeights[j, i] <= (22.1f / terrainData.heightmapScale.y)))
                    newHeights[j, i] = oldHeights[j, i] - holeDepth / terrainData.heightmapScale.y;
                else
                    newHeights[j, i] = oldHeights[j, i];

        terrainData.SetHeights(xCentre - xTranslate, zCentre - zTranslate, newHeights);
    }
}
