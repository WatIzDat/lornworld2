using System;
using UnityEngine;

public class ChunkArray
{
    // array is bottom left to top right (q3 to q1) counting left to right
    private readonly Chunk[] chunks;

    public Chunk Center => chunks[chunks.Length / 2];

    public event Action<ChunkPos, int, TileScriptableObject> ChunkChanged;

    public ChunkArray(int renderDistance)
    {
        int side = 1 + (2 * renderDistance);

        chunks = new Chunk[side * side];

        int[] vals = new int[side];

        for (int i = 0; i < side; i++)
        {
            vals[i] = i - renderDistance;
        }

        for (int i = 0; i < chunks.Length; i++)
        {
            // confusing but modulus changes for every column and divide changes for every row
            Vector2Int pos = new(vals[i % side], vals[i / side]);

            Debug.Log(pos);

            chunks[i] = new Chunk(new ChunkPos(pos));
        }

        Chunk.ChunkChanged += OnChunkChanged;
    }

    private void OnChunkChanged(ChunkPos chunkPos, int index, TileScriptableObject tile)
    {
        ChunkChanged?.Invoke(chunkPos, index, tile);
    }

    public void PopulateChunksWith(Func<ChunkPos, TileScriptableObject[]> generate)
    {
        foreach (Chunk chunk in chunks)
        {
            chunk.PopulateWith(generate);
        }
    }
}
