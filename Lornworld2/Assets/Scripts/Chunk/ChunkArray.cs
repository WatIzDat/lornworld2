using System;
using UnityEngine;

public class ChunkArray
{
    // array is bottom left to top right (q3 to q1) counting left to right
    private Chunk[] chunks;

    private readonly int sideLength;

    public Chunk Center => chunks[chunks.Length / 2];

    public event Action<ChunkPos, int, TileScriptableObject> ChunkChanged;
    public event Action<ChunkPos> ChunkUnloaded;

    public ChunkArray(int renderDistance)
    {
        sideLength = 1 + (2 * renderDistance);

        chunks = new Chunk[sideLength * sideLength];

        int[] vals = new int[sideLength];

        for (int i = 0; i < sideLength; i++)
        {
            vals[i] = i - renderDistance;
        }

        for (int i = 0; i < chunks.Length; i++)
        {
            // confusing but modulus changes for every column and divide changes for every row
            Vector2Int pos = new(vals[i % sideLength], vals[i / sideLength]);

            Debug.Log(pos);

            chunks[i] = new Chunk(new ChunkPos(pos));
        }

        Chunk.ChunkChanged += OnChunkChanged;
    }

    private void OnChunkChanged(ChunkPos chunkPos, int index, TileScriptableObject tile)
    {
        ChunkChanged?.Invoke(chunkPos, index, tile);
    }

    public void ShiftHorizontal(bool shiftLeft, Func<ChunkPos, TileScriptableObject[]> generate)
    {
        Chunk[] newChunks = new Chunk[chunks.Length];

        for (int i = 0; i < chunks.Length; i++)
        {
            if (shiftLeft)
            {
                if (i % sideLength == 0)
                {
                    ChunkUnloaded?.Invoke(chunks[i].chunkPos);
                }

                if ((i + 1) % sideLength == 0)
                {
                    ChunkPos pos = new(chunks[i].chunkPos.pos + Vector2Int.right);

                    Chunk chunk = new(pos);
                    chunk.PopulateWith(generate);

                    newChunks[i] = chunk;
                }
                else
                {
                    newChunks[i] = chunks[i + 1];
                }
            }
        }

        //foreach (Chunk chunk in newChunks)
        //{
        //    Debug.Log(chunk.chunkPos.pos);
        //}

        chunks = newChunks;
    }

    public void PopulateChunksWith(Func<ChunkPos, TileScriptableObject[]> generate)
    {
        foreach (Chunk chunk in chunks)
        {
            chunk.PopulateWith(generate);
        }
    }
}
