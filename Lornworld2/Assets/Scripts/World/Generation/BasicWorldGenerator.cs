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

        return new ChunkData(tiles, new (FeatureScriptableObject feature, Vector2 pos)[] { (FeatureRegistry.Instance.GetEntry(FeatureIds.TreeFeature), Vector2.zero * 8f) });
    }
}
