using MemoryPack;

[MemoryPackable]
[MemoryPackUnion(0, typeof(GameData))]
[MemoryPackUnion(1, typeof(ChunkDataPersistence))]
public partial interface IGameData
{
}
