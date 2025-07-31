public record InventoryItem
{
    public ItemScriptableObject item;
    public int stackSize;

    public InventoryItem(ItemScriptableObject item, int stackSize = 1)
    {
        this.item = item;
        this.stackSize = stackSize;
    }
}
