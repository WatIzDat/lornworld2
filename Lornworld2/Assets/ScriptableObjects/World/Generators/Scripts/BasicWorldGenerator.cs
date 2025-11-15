using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicWorldGenerator", menuName = "Scriptable Objects/World/Generators/Basic")]
public class BasicWorldGenerator : WorldGeneratorScriptableObject
{
    public override ChunkData Generate(ChunkPos pos)
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
        float jitter = 2f;

        for (int y = spacing / 4; y < ChunkManager.ChunkSize - (spacing / 4); y += spacing)
        {
            for (int x = spacing / 4; x < ChunkManager.ChunkSize - (spacing / 4); x += spacing)
            {
                Vector2 featurePos = new(
                    x + jitter * (spacing / 2) * (Random.value - 0.5f),
                    y + jitter * (spacing / 2) * (Random.value - 0.5f));

                if (featurePos.x < 0f ||
                    featurePos.x > ChunkManager.ChunkSize ||
                    featurePos.y < 0f ||
                    featurePos.y > ChunkManager.ChunkSize)
                {
                    continue;
                }

                Vector2Int tilePos = Vector2Int.RoundToInt(featurePos);

                if (tiles[tilePos.y * ChunkManager.ChunkSize + tilePos.x] == TileRegistry.Instance.GetEntry(TileIds.WaterTile))
                {
                    continue;
                }

                features.Add((FeatureRegistry.Instance.GetEntry(FeatureIds.TreeFeature), featurePos));
            }
        }

        return new ChunkData(tiles, features.ToArray());
    }
}
