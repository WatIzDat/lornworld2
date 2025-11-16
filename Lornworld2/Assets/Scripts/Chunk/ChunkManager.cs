using System;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public const int ChunkSize = 16;

    public const int ChunkArea = ChunkSize * ChunkSize;

    [SerializeField]
    private GameObject chunkPrefab;

    [SerializeField]
    private Transform chunkParent;

    [SerializeField]
    private int renderDistance;

    //[SerializeField]
    private Player player;

    [SerializeField]
    private DualGridTilemap tilemap;

    private ChunkArray loadedChunks;

    private IWorldGenerator worldGenerator;

    public int LoadedChunksSideLength => loadedChunks.SideLength;

    public static event Action<Vector2Int> LoadedChunksShifted;

    private void Awake()
    {
        loadedChunks = ChunkArray.AddChunkArrayComponent(gameObject, renderDistance, chunkPrefab, chunkParent, this);

        player = FindFirstObjectByType<Player>();
    }

    private void OnEnable()
    {
        loadedChunks.ChunkChanged += OnChunkChanged;
        //loadedChunks.ChunkUnloaded += OnChunkUnloaded;
    }

    private void OnDisable()
    {
        loadedChunks.ChunkChanged -= OnChunkChanged;
        //loadedChunks.ChunkUnloaded -= OnChunkUnloaded;
    }

    private bool isShiftingChunks;

    // Don't have to check every frame (change later)
    private void Update()
    {
        if (isShiftingChunks)
        {
            return;
        }

        if (Vector2Int.Distance(player.ChunkPos.pos, loadedChunks.Center.chunkPos.pos) >= LoadedChunksSideLength)
        {
            isShiftingChunks = true;

            StartCoroutine(
                loadedChunks.CenterChunksAround(
                    player.ChunkPos,
                    worldGenerator.Generate,
                    () =>
                    {
                        Debug.Log("callback");
                        isShiftingChunks = false;
                    }));

            return;
        }

        // moved off a chunk
        if (player.ChunkPos.pos.x - loadedChunks.Center.chunkPos.pos.x > 0)
        {
            isShiftingChunks = true;
            Debug.Log("moved off chunk");
            
            StartCoroutine(
                loadedChunks.ShiftHorizontal(
                    true,
                    worldGenerator.Generate,
                    () =>
                    {
                        Debug.Log("callback");
                        isShiftingChunks = false;
                        LoadedChunksShifted?.Invoke(Vector2Int.left);
                    }));
        }
        else if (player.ChunkPos.pos.x - loadedChunks.Center.chunkPos.pos.x < 0)
        {
            isShiftingChunks = true;
            Debug.Log("moved off chunk");

            StartCoroutine(
                loadedChunks.ShiftHorizontal(
                    false,
                    worldGenerator.Generate,
                    () =>
                    {
                        Debug.Log("callback");
                        isShiftingChunks = false;
                        LoadedChunksShifted?.Invoke(Vector2Int.right);
                    }));
        }
        else if (player.ChunkPos.pos.y - loadedChunks.Center.chunkPos.pos.y > 0)
        {
            isShiftingChunks = true;
            Debug.Log("moved off chunk");

            StartCoroutine(
                loadedChunks.ShiftVertical(
                    true,
                    worldGenerator.Generate,
                    () =>
                    {
                        Debug.Log("callback");
                        isShiftingChunks = false;
                        LoadedChunksShifted?.Invoke(Vector2Int.down);
                    }));
        }
        else if (player.ChunkPos.pos.y - loadedChunks.Center.chunkPos.pos.y < 0)
        {
            isShiftingChunks = true;
            Debug.Log("moved off chunk");

            StartCoroutine(
                loadedChunks.ShiftVertical(
                    false,
                    worldGenerator.Generate,
                    () =>
                    {
                        Debug.Log("callback");
                        isShiftingChunks = false;
                        LoadedChunksShifted?.Invoke(Vector2Int.up);
                    }));
        }
    }

    private void OnChunkChanged(ChunkPos chunkPos, int index, TileScriptableObject tile)
    {
        //Vector2Int localPos = new(index % ChunkSize, index / ChunkSize);

        //Vector2Int pos = localPos + (chunkPos.pos * ChunkSize);

        //tilemap.SetTile(pos, tile);
    }

    //private void OnChunkUnloaded(ChunkPos chunkPos)
    //{
    //    //Debug.Log(chunkPos.pos);

    //    for (int x = 0; x < ChunkSize; x++)
    //    {
    //        for (int y = 0; y < ChunkSize; y++)
    //        {
    //            Vector2Int localPos = new(x, y);

    //            Vector2Int pos = localPos + (chunkPos.pos * ChunkSize);

    //            tilemap.DeleteTile(pos);
    //        }
    //    }
    //}

    public TileScriptableObject GetTileInChunkAt(ChunkPos chunkPos, Vector2Int tilePos)
    {
        Chunk chunk = FindChunkAt(chunkPos);

        if (chunk == null)
        {
            return null;
        }

        return chunk.GetTile(tilePos);
    }

    public Chunk FindChunkAt(ChunkPos chunkPos)
    {
        return loadedChunks.FindChunkAt(chunkPos);
    }

    public void Generate(IWorldGenerator generator)
    {
        worldGenerator = generator;

        StartCoroutine(loadedChunks.PopulateChunksWith(generator.Generate));
    }
}
