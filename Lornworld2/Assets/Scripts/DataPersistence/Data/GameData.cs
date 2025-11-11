using MemoryPack;
using System.Collections.Generic;
using UnityEngine;

[MemoryPackable]
public partial class GameData : IGameData
{
    public Vector2 playerPosition;
    //public Dictionary<ChunkPos, string> chunkFilePaths;

    public GameData(Vector2 playerPosition)
    {
        this.playerPosition = playerPosition;
        //chunkFilePaths = new Dictionary<ChunkPos, string>();
    }
}
