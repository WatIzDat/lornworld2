using MemoryPack;

[MemoryPackable]
[MemoryPackUnion(0, typeof(PlayerData))]
[MemoryPackUnion(1, typeof(ChunkDataPersistence))]
[MemoryPackUnion(2, typeof(WorldData))]
[MemoryPackUnion(3, typeof(InventoryData))]
[MemoryPackUnion(4, typeof(InventoryItemData))]
[MemoryPackUnion(5, typeof(GameData))]
[MemoryPackUnion(6, typeof(ItemScriptableObjectData))]
public partial interface IGameData
{
}
