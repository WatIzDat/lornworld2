using MemoryPack;
using UnityEngine;

[MemoryPackable]
public partial class GameData
{
    public Vector2 playerPosition;

    public GameData()
    {
        playerPosition = Vector2.zero;
    }
}
