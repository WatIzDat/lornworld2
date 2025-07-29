using UnityEngine;

public class BasicWorldGenerator : IWorldGenerator
{
    public TileScriptableObject[] Generate(ChunkPos pos)
    {
        TileScriptableObject[] tiles = new TileScriptableObject[ChunkManager.ChunkArea];
        
        for (int y = 0; y < ChunkManager.ChunkSize; y++)
        {
            for (int x = 0; x < ChunkManager.ChunkSize; x++)
            {
                float noiseX = ((x / (float)ChunkManager.ChunkSize) - 0.5f + (pos.pos.x));
                float noiseY = ((y / (float)ChunkManager.ChunkSize) - 0.5f + (pos.pos.y));

                // adding large number to avoid symmetry around 0, 0
                float e =
                    1f * Mathf.PerlinNoise(1f * noiseX + 100, 1f * noiseY + 100) +
                  0.5f * Mathf.PerlinNoise(2f * noiseX + 100, 2f * noiseY + 100) +
                 0.25f * Mathf.PerlinNoise(4f * noiseX + 100, 4f * noiseY + 100);

                e /= 1f + 0.5f + 0.25f;

                e = (e * 1.2f) * (e * 1.2f);

                int index = (y * ChunkManager.ChunkSize) + x;

                //Debug.Log(e);

                if (e < 0.5f)
                {
                    tiles[index] = TileRegistry.Instance.GetTile(TileIds.Water);
                }
                else
                {
                    tiles[index] = TileRegistry.Instance.GetTile(TileIds.Grass);
                }
            }
        }

        return tiles;
    }
}
