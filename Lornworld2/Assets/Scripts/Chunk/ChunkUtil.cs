using UnityEngine;

public static class ChunkUtil
{
    public static ChunkPos ToChunkPos(Vector2 pos)
    {
        Vector2Int newPos = new(
            Mathf.FloorToInt(pos.x / ChunkManager.ChunkSize),
            Mathf.FloorToInt(pos.y / ChunkManager.ChunkSize));

        return new ChunkPos(newPos);
    }
}
