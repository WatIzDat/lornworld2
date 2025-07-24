using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    // change to 16 later
    public const int ChunkSize = 2;

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
    }

    private void OnDisable()
    {
        loadedChunks.ChunkChanged -= OnChunkChanged;
    }

    // Don't have to check every frame (change later)
    private void Update()
    {
        // moved off a chunk
        if (Mathf.Abs(player.ChunkPos.pos.x - loadedChunks.Center.chunkPos.pos.x) > 0)
        {
            Debug.Log("x");
        }
        else if (Mathf.Abs(player.ChunkPos.pos.y - loadedChunks.Center.chunkPos.pos.y) > 0)
        {
            Debug.Log("y");
        }
    }

    private void OnChunkChanged(ChunkPos chunkPos, int index, TileScriptableObject tile)
    {
        Vector2Int localPos = new(index % ChunkSize, index / ChunkSize);

        Vector2Int pos = localPos + (chunkPos.pos * ChunkSize);

        tilemap.SetTile(pos, tile);
    }

    public void Generate(IWorldGenerator generator)
    {
        loadedChunks.PopulateChunksWith(generator.Generate);
    }
}
