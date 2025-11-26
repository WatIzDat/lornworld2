using MemoryPack;

[System.Serializable]
public record InventoryItem
{
    public ItemScriptableObject item;

    public int stackSize;

    public InventoryItem(ItemScriptableObject item, int stackSize = 1)
    {
        this.item = item;
        this.stackSize = stackSize;
    }

    public InventoryItem AddStack(int stack)
    {
        if (stackSize + stack <= 0)
        {
            return null;
        }

        return new InventoryItem(item, stackSize + stack);
    }

    public InventoryItem WithStack(int stack)
    {
        return new InventoryItem(item, stack);
    }

    public void UseItem()
    {
        if (item.itemUseBehavior != null)
        {
            item.itemUseBehavior.UseItem();
        }
    }
}

[MemoryPackable]
public partial record InventoryItemData : IGameData
{
    public ItemIdentifier item;
    public int stackSize;

    public InventoryItemData(ItemIdentifier item, int stackSize)
    {
        this.item = item;
        this.stackSize = stackSize;
    }
}
