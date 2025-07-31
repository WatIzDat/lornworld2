public record InventoryItem
{
    public ItemScriptableObject item;
    public int stackSize;

    public InventoryItem(ItemScriptableObject item)
    {
        this.item = item;
        stackSize = 1;
    }
}
