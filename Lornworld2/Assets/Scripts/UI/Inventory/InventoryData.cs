using MemoryPack;
using System.Collections.Generic;

[MemoryPackable]
public partial class InventoryData : IGameData
{
    public IEnumerable<InventoryItemData> items;

    public InventoryData(IEnumerable<InventoryItemData> items)
    {
        this.items = items;
    }
}
