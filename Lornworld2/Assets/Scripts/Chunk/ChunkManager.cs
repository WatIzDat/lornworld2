using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private Func<ChunkPos, ChunkData> worldGenerator;

    public int LoadedChunksSideLength => loadedChunks.SideLength;

    public static event Action<Vector2Int> LoadedChunksShifted;

    public static event Action InitialChunksGenerated;
    public bool AreInitialChunksGenerated { get; private set; }

    public bool IsShiftingChunks { get; private set; }

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

    // Don't have to check every frame (change later)
    private void Update()
    {
        if (IsShiftingChunks || worldGenerator == null)
        {
            return;
        }

        if (Vector2Int.Distance(player.ChunkPos.pos, loadedChunks.Center.chunkPos.pos) >= LoadedChunksSideLength)
        {
            IsShiftingChunks = true;

            Vector2Int translation = loadedChunks.Center.chunkPos.pos - player.ChunkPos.pos;

            StartCoroutine(
                loadedChunks.CenterChunksAround(
                    player.ChunkPos,
                    worldGenerator,
                    () =>
                    {
                        Debug.Log("callback");
                        IsShiftingChunks = false;
                        LoadedChunksShifted?.Invoke(translation);
                    }));

            return;
        }

        // moved off a chunk
        if (player.ChunkPos.pos.x - loadedChunks.Center.chunkPos.pos.x > 0)
        {
            IsShiftingChunks = true;
            Debug.Log("moved off chunk");
            
            StartCoroutine(
                loadedChunks.ShiftHorizontal(
                    true,
                    worldGenerator,
                    () =>
                    {
                        Debug.Log("callback");
                        IsShiftingChunks = false;
                        LoadedChunksShifted?.Invoke(Vector2Int.left);
                    }));
        }
        else if (player.ChunkPos.pos.x - loadedChunks.Center.chunkPos.pos.x < 0)
        {
            IsShiftingChunks = true;
            Debug.Log("moved off chunk");

            StartCoroutine(
                loadedChunks.ShiftHorizontal(
                    false,
                    worldGenerator,
                    () =>
                    {
                        Debug.Log("callback");
                        IsShiftingChunks = false;
                        LoadedChunksShifted?.Invoke(Vector2Int.right);
                    }));
        }
        else if (player.ChunkPos.pos.y - loadedChunks.Center.chunkPos.pos.y > 0)
        {
            IsShiftingChunks = true;
            Debug.Log("moved off chunk");

            StartCoroutine(
                loadedChunks.ShiftVertical(
                    true,
                    worldGenerator,
                    () =>
                    {
                        Debug.Log("callback");
                        IsShiftingChunks = false;
                        LoadedChunksShifted?.Invoke(Vector2Int.down);
                    }));
        }
        else if (player.ChunkPos.pos.y - loadedChunks.Center.chunkPos.pos.y < 0)
        {
            IsShiftingChunks = true;
            Debug.Log("moved off chunk");

            StartCoroutine(
                loadedChunks.ShiftVertical(
                    false,
                    worldGenerator,
                    () =>
                    {
                        Debug.Log("callback");
                        IsShiftingChunks = false;
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

    public Chunk GetRandomLoadedChunk()
    {
        return loadedChunks.GetRandomChunk();
    }

    public Vector2? GetSpawnpoint(bool createSceneEntrance = false)
    {
        if (!AreInitialChunksGenerated)
        {
            return null;
        }

        Vector2? spawnpoint = null;
        ChunkPos chunkPos = new(Vector2Int.zero);
        int i = 0;

        while (spawnpoint == null)
        {
            Chunk chunk = FindChunkAt(chunkPos);

            List<Vector2> spawnableTiles = new();

            for (int y = 0; y < ChunkSize; y++)
            {
                for (int x = 0; x < ChunkSize; x++)
                {
                    Vector2Int tilePos = new(x, y);

                    TileScriptableObject tile = chunk.GetTile(tilePos);

                    if (tile.isWalkable)
                    {
                        spawnableTiles.Add(tilePos + (chunk.chunkPos.pos * ChunkSize));
                    }
                }
            }

            if (spawnableTiles.Count > 0)
            {
                spawnpoint = spawnableTiles[UnityEngine.Random.Range(0, spawnableTiles.Count)] + (Vector2.one * 0.5f);

                if (createSceneEntrance)
                {
                    Feature.Create(
                        chunk,
                        FeatureRegistry.Instance.GetEntry(FeatureIds.SceneEntranceFeature),
                        (Vector2)spawnpoint,
                        new SceneEntranceFeatureData(0, ScenePersistentInfo.PrevSceneId));
                }
            }
            else
            {
                int spiralSideLength = (i / 2) + 1;

                // Checks in a spiral
                if (i % 4 == 0)
                {
                    chunkPos = new ChunkPos(new Vector2Int(chunkPos.pos.x, chunkPos.pos.y + spiralSideLength));
                }
                else if (i % 4 == 1)
                {
                    chunkPos = new ChunkPos(new Vector2Int(chunkPos.pos.x + spiralSideLength, chunkPos.pos.y));
                }
                else if (i % 4 == 2)
                {
                    chunkPos = new ChunkPos(new Vector2Int(chunkPos.pos.x, -chunkPos.pos.y));
                }
                else if (i % 4 == 3)
                {
                    chunkPos = new ChunkPos(new Vector2Int(-chunkPos.pos.x, chunkPos.pos.y));
                }
            }

            i++;
        }

        Debug.Log(spawnpoint);

        return spawnpoint;
    }

    public void Generate(Func<ChunkPos, ChunkData> generator)
    {
        worldGenerator = generator;

        StartCoroutine(loadedChunks.PopulateChunksWith(generator, () =>
        {
            AreInitialChunksGenerated = true;
            InitialChunksGenerated?.Invoke();
        }));
    }
}
