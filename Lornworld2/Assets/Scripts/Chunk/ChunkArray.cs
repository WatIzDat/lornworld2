using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkArray : MonoBehaviour
{
    // array is bottom left to top right (q3 to q1) counting left to right
    private Chunk[] chunks;

    private readonly Queue<GameObject> unloadedChunks = new();

    public int SideLength { get; private set; }

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

        chunkArray.SideLength = 1 + (2 * renderDistance);

        chunkArray.chunks = new Chunk[chunkArray.SideLength * chunkArray.SideLength];

        int[] vals = new int[chunkArray.SideLength];

        for (int i = 0; i < chunkArray.SideLength; i++)
        {
            vals[i] = i - renderDistance;
        }

        for (int i = 0; i < chunkArray.chunks.Length; i++)
        {
            // confusing but modulus changes for every column and divide changes for every row
            Vector2Int pos = new(vals[i % chunkArray.SideLength], vals[i / chunkArray.SideLength]);

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

    private void PopulateNewChunk(ChunkPos pos, Func<ChunkPos, ChunkData> generate, int index, Action<Chunk, int> callback)
    {
        Chunk chunk = PoolOrCreate(pos);

        maxDisplayOrder++;
        chunk.SetDisplayOrder(maxDisplayOrder);

        chunk.PopulateWith(generate, () => callback(chunk, index));

        // hacky solution to avoid recalculating chunk boundaries twice by drawing new chunk boundaries on top of old one
        // however, TODO: decrease ALL display orders when it hits the max of short max value

        //return chunk;
    }

    public IEnumerator CenterChunksAround(ChunkPos centerPos, Func<ChunkPos, ChunkData> generate, Action callback)
    {
        Chunk[] newChunks = new Chunk[chunks.Length];
        int chunksPopulated = 0;
        List<int> pendingChunkUpdateIndices = new(SideLength);

        for (int i = 0; i < chunks.Length; i++)
        {
            ChunkUnloaded?.Invoke(i);

            ChunkPos pos = new(new Vector2Int(
                centerPos.pos.x - (SideLength / 2) + (i % SideLength),
                centerPos.pos.y - (SideLength / 2) + (i / SideLength)));

            PopulateNewChunk(pos, generate, i, (chunk, i) =>
            {
                newChunks[i] = chunk;
                chunksPopulated++;
                pendingChunkUpdateIndices.Add(i);
            });
        }

        while (chunksPopulated != chunks.Length)
        {
            yield return null;
        }

        chunks = newChunks;

        foreach (int index in pendingChunkUpdateIndices)
        {
            newChunks[index].SetDisplayTiles();
        }

        callback();
    }

    // TODO: make shifting right and shifting down iterate backwards to avoid instantiating chunks
    public IEnumerator ShiftHorizontal(bool shiftLeft, Func<ChunkPos, ChunkData> generate, Action callback)
    {
        Chunk[] newChunks = new Chunk[chunks.Length];
        int chunksPopulated = 0;
        List<int> pendingChunkUpdateIndices = new(SideLength);

        for (int i = 0; i < chunks.Length; i++)
        {
            //Debug.Log(i);
            void populateCallback(Chunk chunk, int i)
            {
                //Debug.Log(i);
                newChunks[i] = chunk;
                chunksPopulated++;
                pendingChunkUpdateIndices.Add(i);
            }

            if (shiftLeft)
            {
                if (i % SideLength == 0)
                {
                    ChunkUnloaded?.Invoke(i);
                }

                if ((i + 1) % SideLength == 0)
                {
                    ChunkPos pos = new(chunks[i].chunkPos.pos + Vector2Int.right);

                    PopulateNewChunk(pos, generate, i, populateCallback);

                    //newChunks[i] = chunk;
                    //pendingChunkUpdateIndices.Add(i);
                }
                else
                {
                    newChunks[i] = chunks[i + 1];

                    chunksPopulated++;
                }
            }
            else
            {
                if ((i + 1) % SideLength == 0)
                {
                    ChunkUnloaded?.Invoke(i);
                }

                if (i % SideLength == 0)
                {
                    ChunkPos pos = new(chunks[i].chunkPos.pos + Vector2Int.left);

                    PopulateNewChunk(pos, generate, i, populateCallback);

                    //newChunks[i] = chunk;
                    //pendingChunkUpdateIndices.Add(i);
                }
                else
                {
                    newChunks[i] = chunks[i - 1];

                    chunksPopulated++;
                }
            }
        }

        while (chunksPopulated != chunks.Length)
        {
            yield return null;
        }

        chunks = newChunks;

        foreach (int index in pendingChunkUpdateIndices)
        {
            newChunks[index].SetDisplayTiles();
        }

        callback();
    }

    public IEnumerator ShiftVertical(bool shiftDown, Func<ChunkPos, ChunkData> generate, Action callback)
    {
        Chunk[] newChunks = new Chunk[chunks.Length];
        int chunksPopulated = 0;
        List<int> pendingChunkUpdateIndices = new(SideLength);

        for (int i = 0; i < chunks.Length; i++)
        {
            void populateCallback(Chunk chunk, int i)
            {
                newChunks[i] = chunk;
                chunksPopulated++;
                pendingChunkUpdateIndices.Add(i);
            }

            if (shiftDown)
            {
                if (i / SideLength == 0)
                {
                    ChunkUnloaded?.Invoke(i);
                }

                if (i / SideLength == SideLength - 1)
                {
                    Debug.Log(chunks[i]);
                    ChunkPos pos = new(chunks[i].chunkPos.pos + Vector2Int.up);

                    PopulateNewChunk(pos, generate, i, populateCallback);

                    //newChunks[i] = chunk;
                    //pendingChunkUpdateIndices.Add(i);
                }
                else
                {
                    newChunks[i] = chunks[i + SideLength];

                    chunksPopulated++;
                }
            }
            else
            {
                if (i / SideLength == SideLength - 1)
                {
                    ChunkUnloaded?.Invoke(i);
                }

                if (i / SideLength == 0)
                {
                    ChunkPos pos = new(chunks[i].chunkPos.pos + Vector2Int.down);

                    PopulateNewChunk(pos, generate, i, populateCallback);

                    //newChunks[i] = chunk;
                    //pendingChunkUpdateIndices.Add(i);
                }
                else
                {
                    newChunks[i] = chunks[i - SideLength];

                    chunksPopulated++;
                }
            }
        }

        while (chunksPopulated != chunks.Length)
        {
            yield return null;
        }

        chunks = newChunks;

        foreach (int index in pendingChunkUpdateIndices)
        {
            newChunks[index].SetDisplayTiles();
        }

        callback();
    }

    //private int chunksPopulated;

    //private void IncrementChunksPopulated()
    //{
    //    chunksPopulated++;
    //}

    public IEnumerator PopulateChunksWith(Func<ChunkPos, ChunkData> generate)
    {
        //TileScriptableObject[][] tilesInChunks = new TileScriptableObject[chunks.Length][];

        int chunksPopulated = 0;

        for (int i = 0; i < chunks.Length; i++)
        {
            //TileScriptableObject[] tiles = 
            chunks[i].PopulateWith(generate, () => chunksPopulated++);

            //tilesInChunks[i] = tiles;
        }

        while (chunksPopulated != chunks.Length)
        {
            yield return null;
        }

        foreach (Chunk chunk in chunks)
        {
            chunk.SetDisplayTiles();
        }

        //return tilesInChunks;
    }

    //private void Update()
    //{
    //    if (chunksPopulated == chunks.Length)
    //    {
    //        foreach (Chunk chunk in chunks)
    //        {
    //            chunk.SetDisplayTiles();
    //        }

    //        chunksPopulated = 0;
    //    }
    //}
}
