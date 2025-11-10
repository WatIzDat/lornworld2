using MemoryPack;
using UnityEngine;

[MemoryPackable]
public partial record ChunkPos
{
    public Vector2Int pos;

    public ChunkPos(Vector2Int pos)
    {
        this.pos = pos;
    }
}
