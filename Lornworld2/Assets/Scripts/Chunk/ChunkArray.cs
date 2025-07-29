using System;
using System.Collections.Generic;
using UnityEngine;

public class ChunkArray : MonoBehaviour
{
    // array is bottom left to top right (q3 to q1) counting left to right
    private Chunk[] chunks;

    private readonly Queue<GameObject> unloadedChunks = new();

    private int sideLength;

    private int maxDisplayOrder;

    //[SerializeField]
    private GameObject chunkPrefab;
    private Transform chunkParent;
    private ChunkManager chunkManager;

    public Chunk Center => chunks[chunks.Length / 2];

    public event Action<ChunkPos, int, TileScriptableObject> ChunkChanged;
    public event Action<int> ChunkUnloaded;

    public static ChunkArray AddChunkArrayComponent(GameObject gameObject, int renderDistance, GameObject chunkPrefab, Transform chunkParent, ChunkManager chunkManager)
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

            //Debug.Log(pos);

            //chunks[i] = new Chunk(new ChunkPos(pos));
            chunkArray.chunks[i] = Chunk.Create(new ChunkPos(pos), chunkPrefab, chunkParent, chunkManager);
        }

        Chunk.ChunkChanged += chunkArray.OnChunkChanged;

        chunkArray.ChunkUnloaded += chunkArray.OnChunkUnloaded;

        chunkArray.chunkPrefab = chunkPrefab;
        chunkArray.chunkParent = chunkParent;
        chunkArray.chunkManager = chunkManager;

        return chunkArray;
    }

    public Chunk FindChunkAt(ChunkPos chunkPos)
    {
        foreach (Chunk chunk in chunks)
        {
            if (chunk.chunkPos == chunkPos)
            {
                return chunk;
            }
        }

        return null;
    }

    private void OnChunkUnloaded(int index)
    {
        chunks[index].gameObject.SetActive(false);
        //chunks[index].transform.GetChild(0).GetChild(1).GetComponent<TilemapRenderer>().enabled = false;

        unloadedChunks.Enqueue(chunks[index].gameObject);
    }

    private void OnChunkChanged(ChunkPos chunkPos, int index, TileScriptableObject tile)
    {
        ChunkChanged?.Invoke(chunkPos, index, tile);
    }

    private Chunk PoolOrCreate(ChunkPos pos)
    {
        bool unloadedChunkExists = unloadedChunks.TryDequeue(out GameObject unloadedChunk);

        if (!unloadedChunkExists)
        {
            Debug.LogWarning("Unloaded chunk doesn't exist, instantiating chunk instead");

            return Chunk.Create(pos, chunkPrefab, chunkParent, chunkManager);
        }

        return Chunk.Pool(pos, unloadedChunk);
    }

    private Chunk PopulateNewChunk(ChunkPos pos, Func<ChunkPos, TileScriptableObject[]> generate)
    {
        Chunk chunk = PoolOrCreate(pos);
        chunk.PopulateWith(generate);

        // hacky solution to avoid recalculating chunk boundaries twice by drawing new chunk boundaries on top of old one
        // however, TODO: decrease ALL display orders when it hits the max of short max value
        maxDisplayOrder++;
        chunk.SetDisplayOrder(maxDisplayOrder);

        return chunk;
    }

    // TODO: make shifting right and shifting down iterate backwards to avoid instantiating chunks
    public void ShiftHorizontal(bool shiftLeft, Func<ChunkPos, TileScriptableObject[]> generate)
    {
        Chunk[] newChunks = new Chunk[chunks.Length];
        List<int> pendingChunkUpdateIndices = new(sideLength);

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

                    Chunk chunk = PopulateNewChunk(pos, generate);

                    newChunks[i] = chunk;
                    pendingChunkUpdateIndices.Add(i);
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

                    Chunk chunk = PopulateNewChunk(pos, generate);

                    newChunks[i] = chunk;
                    pendingChunkUpdateIndices.Add(i);
                }
                else
                {
                    newChunks[i] = chunks[i - 1];
                }
            }
        }

        chunks = newChunks;

        foreach (int index in pendingChunkUpdateIndices)
        {
            newChunks[index].SetDisplayTiles();
        }
    }

    public void ShiftVertical(bool shiftDown, Func<ChunkPos, TileScriptableObject[]> generate)
    {
        Chunk[] newChunks = new Chunk[chunks.Length];
        List<int> pendingChunkUpdateIndices = new(sideLength);

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

                    Chunk chunk = PopulateNewChunk(pos, generate);

                    newChunks[i] = chunk;
                    pendingChunkUpdateIndices.Add(i);
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

                    Chunk chunk = PopulateNewChunk(pos, generate);

                    newChunks[i] = chunk;
                    pendingChunkUpdateIndices.Add(i);
                }
                else
                {
                    newChunks[i] = chunks[i - sideLength];
                }
            }
        }

        chunks = newChunks;

        foreach (int index in pendingChunkUpdateIndices)
        {
            newChunks[index].SetDisplayTiles();
        }
    }

    public TileScriptableObject[][] PopulateChunksWith(Func<ChunkPos, TileScriptableObject[]> generate)
    {
        TileScriptableObject[][] tilesInChunks = new TileScriptableObject[chunks.Length][];

        for (int i = 0; i < chunks.Length; i++)
        {
            TileScriptableObject[] tiles = chunks[i].PopulateWith(generate);

            tilesInChunks[i] = tiles;
        }

        foreach (Chunk chunk in chunks)
        {
            chunk.SetDisplayTiles();
        }

        return tilesInChunks;
    }
}
