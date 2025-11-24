using MemoryPack;

[MemoryPackable]
[MemoryPackUnion(0, typeof(PlayerData))]
[MemoryPackUnion(1, typeof(ChunkDataPersistence))]
[MemoryPackUnion(2, typeof(WorldData))]
public partial interface IGameData
{
}
