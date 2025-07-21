using UnityEngine;

public static class ChunkUtil
{
    public static ChunkPos ToChunkPos(Vector2 pos)
    {
        Vector2Int newPos = new((int)(pos.x / ChunkManager.ChunkSize), (int)(pos.y / ChunkManager.ChunkSize));

        return new ChunkPos(newPos);
    }
}
