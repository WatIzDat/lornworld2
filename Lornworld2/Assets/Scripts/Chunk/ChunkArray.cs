using System;
using UnityEngine;

public class ChunkArray : MonoBehaviour
{
    // array is bottom left to top right (q3 to q1) counting left to right
    private Chunk[] chunks;

    private int sideLength;

    //[SerializeField]
    private GameObject chunkPrefab;
    private Transform chunkParent;

    public Chunk Center => chunks[chunks.Length / 2];

    public event Action<ChunkPos, int, TileScriptableObject> ChunkChanged;
    public event Action<int> ChunkUnloaded;

    //public ChunkArray(int renderDistance)
    //{
    //    sideLength = 1 + (2 * renderDistance);

    //    chunks = new Chunk[sideLength * sideLength];

    //    int[] vals = new int[sideLength];

    //    for (int i = 0; i < sideLength; i++)
    //    {
    //        vals[i] = i - renderDistance;
    //    }

    //    for (int i = 0; i < chunks.Length; i++)
    //    {
    //        // confusing but modulus changes for every column and divide changes for every row
    //        Vector2Int pos = new(vals[i % sideLength], vals[i / sideLength]);

    //        Debug.Log(pos);

    //        //chunks[i] = new Chunk(new ChunkPos(pos));
    //        chunks[i] = Instantiate(
    //            chunkPrefab,
    //            new Vector3(pos.x, pos.y, 0) * ChunkManager.ChunkSize,
    //            Quaternion.identity);
    //    }

    //    Chunk.ChunkChanged += OnChunkChanged;
    //}

    public static ChunkArray AddChunkArrayComponent(GameObject gameObject, int renderDistance, GameObject chunkPrefab, Transform chunkParent)
    {
        ChunkArray chunkArray = gameObject.AddComponent<ChunkArray>();

        chunkArray.sideLength = 1 + (2 * renderDistance);

        chunkArray.chunks = new Chunk[chunkArray.sideLength * chunkArray.sideLength];

        int[] vals = new int[chunkArray.sideLength];

        for (int i = 0; i < chunkArray.sideLength; i++)
        {
            vals[i] = i - renderDistance;
        }

        for (int i = 0; i < chunkArray.chunks.Length; i++)
        {
            // confusing but modulus changes for every column and divide changes for every row
            Vector2Int pos = new(vals[i % chunkArray.sideLength], vals[i / chunkArray.sideLength]);

            Debug.Log(pos);

            //chunks[i] = new Chunk(new ChunkPos(pos));
            chunkArray.chunks[i] = Chunk.Create(new ChunkPos(pos), chunkPrefab, chunkParent);
        }

        Chunk.ChunkChanged += chunkArray.OnChunkChanged;

        chunkArray.ChunkUnloaded += chunkArray.OnChunkUnloaded;

        chunkArray.chunkPrefab = chunkPrefab;
        chunkArray.chunkParent = chunkParent;

        return chunkArray;
    }

    private void OnChunkUnloaded(int index)
    {
        chunks[index].gameObject.SetActive(false);
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
                    ChunkUnloaded?.Invoke(i);
                }

                if ((i + 1) % sideLength == 0)
                {
                    ChunkPos pos = new(chunks[i].chunkPos.pos + Vector2Int.right);

                    Chunk chunk = Chunk.Create(pos, chunkPrefab, chunkParent);
                    chunk.PopulateWith(generate);

                    newChunks[i] = chunk;
                }
                else
                {
                    newChunks[i] = chunks[i + 1];
                }
            }
            else
            {
                if ((i + 1) % sideLength == 0)
                {
                    ChunkUnloaded?.Invoke(i);
                }

                if (i % sideLength == 0)
                {
                    ChunkPos pos = new(chunks[i].chunkPos.pos + Vector2Int.left);

                    Chunk chunk = Chunk.Create(pos, chunkPrefab, chunkParent);
                    chunk.PopulateWith(generate);

                    newChunks[i] = chunk;
                }
                else
                {
                    newChunks[i] = chunks[i - 1];
                }
            }
        }

        //foreach (Chunk chunk in newChunks)
        //{
        //    Debug.Log(chunk.chunkPos.pos);
        //}

        chunks = newChunks;
    }

    public void ShiftVertical(bool shiftDown, Func<ChunkPos, TileScriptableObject[]> generate)
    {
        Chunk[] newChunks = new Chunk[chunks.Length];

        for (int i = 0; i < chunks.Length; i++)
        {
            if (shiftDown)
            {
                if (i / sideLength == 0)
                {
                    ChunkUnloaded?.Invoke(i);
                }

                if (i / sideLength == sideLength - 1)
                {
                    ChunkPos pos = new(chunks[i].chunkPos.pos + Vector2Int.up);

                    Chunk chunk = Chunk.Create(pos, chunkPrefab, chunkParent);
                    chunk.PopulateWith(generate);

                    newChunks[i] = chunk;
                }
                else
                {
                    newChunks[i] = chunks[i + sideLength];
                }
            }
            else
            {
                if (i / sideLength == sideLength - 1)
                {
                    ChunkUnloaded?.Invoke(i);
                }

                if (i / sideLength == 0)
                {
                    ChunkPos pos = new(chunks[i].chunkPos.pos + Vector2Int.down);

                    Chunk chunk = Chunk.Create(pos, chunkPrefab, chunkParent);
                    chunk.PopulateWith(generate);

                    newChunks[i] = chunk;
                }
                else
                {
                    newChunks[i] = chunks[i - sideLength];
                }
            }
        }

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
