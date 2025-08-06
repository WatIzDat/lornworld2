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

    [SerializeField]
    private Player player;

    [SerializeField]
    private DualGridTilemap tilemap;

    private ChunkArray loadedChunks;

    private void Awake()
    {
        loadedChunks = ChunkArray.AddChunkArrayComponent(gameObject, renderDistance, chunkPrefab, chunkParent, this);
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
        IWorldGenerator generator = new BasicWorldGenerator();

        // moved off a chunk
        if (player.ChunkPos.pos.x - loadedChunks.Center.chunkPos.pos.x > 0)
        {
            loadedChunks.ShiftHorizontal(true, generator.Generate);
        }
        else if (player.ChunkPos.pos.x - loadedChunks.Center.chunkPos.pos.x < 0)
        {
            loadedChunks.ShiftHorizontal(false, generator.Generate);
        }
        else if (player.ChunkPos.pos.y - loadedChunks.Center.chunkPos.pos.y > 0)
        {
            loadedChunks.ShiftVertical(true, generator.Generate);
        }
        else if (player.ChunkPos.pos.y - loadedChunks.Center.chunkPos.pos.y < 0)
        {
            loadedChunks.ShiftVertical(false, generator.Generate);
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

    public TileScriptableObject[][] Generate(IWorldGenerator generator)
    {
        return loadedChunks.PopulateChunksWith(generator.Generate);
    }
}
