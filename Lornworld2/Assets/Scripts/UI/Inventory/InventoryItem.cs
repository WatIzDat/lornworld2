public record InventoryItem
{
    public ItemScriptableObject Item { get; }

    public int StackSize { get; }

    public InventoryItem(ItemScriptableObject item, int stackSize = 1)
    {
        Item = item;
        StackSize = stackSize;
    }
}
