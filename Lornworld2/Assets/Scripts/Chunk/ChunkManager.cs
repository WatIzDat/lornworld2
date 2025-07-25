using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    // change to 16 later
    public const int ChunkSize = 4;

    public const int ChunkArea = ChunkSize * ChunkSize;

    [SerializeField]
    private int renderDistance;

    [SerializeField]
    private PlayerInfo player;

    [SerializeField]
    private DualGridTilemap tilemap;

    private ChunkArray loadedChunks;

    private void Awake()
    {
        loadedChunks = new ChunkArray(renderDistance);
    }

    private void OnEnable()
    {
        loadedChunks.ChunkChanged += OnChunkChanged;
        loadedChunks.ChunkUnloaded += OnChunkUnloaded;
    }

    private void OnDisable()
    {
        loadedChunks.ChunkChanged -= OnChunkChanged;
        loadedChunks.ChunkUnloaded -= OnChunkUnloaded;
    }

    // Don't have to check every frame (change later)
    private void Update()
    {
        IWorldGenerator generator = new DebugWorldGenerator();

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
        Vector2Int localPos = new(index % ChunkSize, index / ChunkSize);

        Vector2Int pos = localPos + (chunkPos.pos * ChunkSize);

        tilemap.SetTile(pos, tile);
    }

    private void OnChunkUnloaded(ChunkPos chunkPos)
    {
        //Debug.Log(chunkPos.pos);

        for (int x = 0; x < ChunkSize; x++)
        {
            for (int y = 0; y < ChunkSize; y++)
            {
                Vector2Int localPos = new(x, y);

                Vector2Int pos = localPos + (chunkPos.pos * ChunkSize);

                tilemap.DeleteTile(pos);
            }
        }
    }

    public void Generate(IWorldGenerator generator)
    {
        loadedChunks.PopulateChunksWith(generator.Generate);
    }
}
