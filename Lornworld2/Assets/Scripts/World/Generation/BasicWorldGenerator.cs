using System.Collections.Generic;
using UnityEngine;

public class BasicWorldGenerator : IWorldGenerator
{
    public ChunkData Generate(ChunkPos pos)
    {
        TileScriptableObject[] tiles = new TileScriptableObject[ChunkManager.ChunkArea];
        
        for (int y = 0; y < ChunkManager.ChunkSize; y++)
        {
            for (int x = 0; x < ChunkManager.ChunkSize; x++)
            {
                float noiseX = ((x / (float)ChunkManager.ChunkSize) - 0.5f + (pos.pos.x)) / 5f;
                float noiseY = ((y / (float)ChunkManager.ChunkSize) - 0.5f + (pos.pos.y)) / 5f;

                // adding large number to avoid symmetry around 0, 0
                float e =
                    1f * Mathf.PerlinNoise(1f * noiseX + 100, 1f * noiseY + 100) +
                  0.5f * Mathf.PerlinNoise(2f * noiseX + 100, 2f * noiseY + 100) +
                 0.25f * Mathf.PerlinNoise(4f * noiseX + 100, 4f * noiseY + 100);

                e /= 1f + 0.5f + 0.25f;

                e = (e * 1.2f) * (e * 1.2f);

                int index = (y * ChunkManager.ChunkSize) + x;

                //Debug.Log(e);

                if (e < 0.2f)
                {
                    tiles[index] = TileRegistry.Instance.GetEntry(TileIds.WaterTile);
                }
                else
                {
                    tiles[index] = TileRegistry.Instance.GetEntry(TileIds.GrassTile);
                }
            }
        }

        List<(FeatureScriptableObject feature, Vector2 pos)> features = new();

        int spacing = 4;
        float jitter = 1f;

        for (int y = 0; y < ChunkManager.ChunkSize; y += spacing)
        {
            for (int x = 0; x < ChunkManager.ChunkSize; x += spacing)
            {
                Vector2 featurePos = new(
                    x + jitter * spacing * (Random.value - 0.5f),
                    y + jitter * spacing * (Random.value - 0.5f));

                Vector2Int tilePos = new(
                    Mathf.RoundToInt(featurePos.x) % ChunkManager.ChunkSize,
                    Mathf.RoundToInt(featurePos.y) % ChunkManager.ChunkSize);

                Debug.Log(tilePos.y * ChunkManager.ChunkSize + tilePos.x);

                if (tilePos.y * ChunkManager.ChunkSize + tilePos.x < 0 ||
                    tilePos.y * ChunkManager.ChunkSize + tilePos.x > ChunkManager.ChunkArea || 
                    tiles[tilePos.y * ChunkManager.ChunkSize + tilePos.x] == TileRegistry.Instance.GetEntry(TileIds.WaterTile))
                {
                    continue;
                }

                features.Add((FeatureRegistry.Instance.GetEntry(FeatureIds.TreeFeature), featurePos));
            }
        }

        return new ChunkData(tiles, features.ToArray());
    }
}
