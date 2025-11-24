using MemoryPack;
using System.Collections.Generic;
using UnityEngine;

[MemoryPackable]
public partial class PlayerData : IGameData
{
    public Vector2 playerPosition;
    //public Dictionary<ChunkPos, string> chunkFilePaths;

    public PlayerData(Vector2 playerPosition)
    {
        this.playerPosition = playerPosition;
        //chunkFilePaths = new Dictionary<ChunkPos, string>();
    }
}
