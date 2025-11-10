using MemoryPack;
using System.Collections.Generic;
using UnityEngine;

[MemoryPackable]
public partial class GameData
{
    public Vector2 playerPosition;
    public Dictionary<ChunkPos, ChunkDataPersistence> chunks;

    public GameData()
    {
        playerPosition = Vector2.zero;
        chunks = new Dictionary<ChunkPos, ChunkDataPersistence>();
    }
}
