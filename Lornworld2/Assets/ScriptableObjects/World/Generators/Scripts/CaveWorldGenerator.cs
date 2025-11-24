using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CaveWorldGenerator", menuName = "Scriptable Objects/World/Generators/Cave")]
public class CaveWorldGenerator : WorldGeneratorScriptableObject
{
    [SerializeField]
    [Range(0, 1)]
    private float randomFillPercent;

    public override Func<ChunkPos, ChunkData> GetGenerator(int seed)
    {
        return pos =>
        {
            TileScriptableObject[] tiles = new TileScriptableObject[ChunkManager.ChunkArea];

            for (int y = 0; y < ChunkManager.ChunkSize; y++)
            {
                for (int x = 0; x < ChunkManager.ChunkSize; x++)
                {
                    float noiseX = ((x / (float)ChunkManager.ChunkSize) - 0.5f + (pos.pos.x)) / 0.75f;
                    float noiseY = ((y / (float)ChunkManager.ChunkSize) - 0.5f + (pos.pos.y)) / 0.75f;

                    float randValue = Mathf.PerlinNoise(noiseX + 100, noiseY + 100);

                    int index = (y * ChunkManager.ChunkSize) + x;

                    //Debug.Log(noiseX + " " + noiseY);

                    tiles[index] = randValue < randomFillPercent
                        ? TileRegistry.Instance.GetEntry(TileIds.WaterTile)
                        : TileRegistry.Instance.GetEntry(TileIds.GrassTile);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                tiles = SmoothMap(tiles, pos);
            }

            return new ChunkData(tiles, System.Array.Empty<(FeatureScriptableObject, Vector2, FeatureData)>());
        };
    }

    private TileScriptableObject[] SmoothMap(TileScriptableObject[] tiles, ChunkPos pos)
    {
        TileScriptableObject[] newTiles = new TileScriptableObject[ChunkManager.ChunkArea];

        for (int y = 0; y < ChunkManager.ChunkSize; y++)
        {
            for (int x = 0; x < ChunkManager.ChunkSize; x++)
            {
                int index = (y * ChunkManager.ChunkSize) + x;

                int wallCount = GetSurroundingWallCount(tiles, x, y, pos);

                //Debug.Log(wallCount);

                if (wallCount > 4)
                {
                    newTiles[index] = TileRegistry.Instance.GetEntry(TileIds.WaterTile);
                }
                else if (wallCount < 4)
                {
                    newTiles[index] = TileRegistry.Instance.GetEntry(TileIds.GrassTile);
                }
                else
                {
                    newTiles[index] = tiles[index];
                }
            }
        }

        return newTiles;
    }

    private int GetSurroundingWallCount(TileScriptableObject[] tiles, int x, int y, ChunkPos pos)
    {
        int wallCount = 0;

        for (int neighborX = x - 1; neighborX <= x + 1; neighborX++)
        {
            for (int neighborY = y - 1; neighborY <= y + 1; neighborY++)
            {
                //Debug.Log(neighborX + " " + neighborY);
                if (neighborX != x || neighborY != y)
                {
                    if (neighborX >= 0 && neighborX < ChunkManager.ChunkSize && neighborY >= 0 && neighborY < ChunkManager.ChunkSize)
                    {
                        int index = (neighborY * ChunkManager.ChunkSize) + neighborX;

                        wallCount += tiles[index] == TileRegistry.Instance.GetEntry(TileIds.WaterTile) ? 1 : 0;
                    }
                    else
                    {
                        float noiseX = ((x / (float)ChunkManager.ChunkSize) - 0.5f + (pos.pos.x)) / 0.75f;
                        float noiseY = ((y / (float)ChunkManager.ChunkSize) - 0.5f + (pos.pos.y)) / 0.75f;

                        float randValue = Mathf.PerlinNoise(noiseX + 100, noiseY + 100);

                        wallCount += randValue < randomFillPercent ? 1 : 0;
                    }
                }
            }
        }

        return wallCount;
    }
}
