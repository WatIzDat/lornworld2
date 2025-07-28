using UnityEngine;

public record ChunkPos
{
    public Vector2Int pos;

    public ChunkPos(Vector2Int pos)
    {
        this.pos = pos;
    }
}
