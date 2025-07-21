using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    // change to 16 later
    public const int ChunkSize = 2;

    [SerializeField]
    private int renderDistance;

    [SerializeField]
    private PlayerInfo player;

    private ChunkArray loadedChunks;

    private void Awake()
    {
        loadedChunks = new ChunkArray(renderDistance);
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
}
