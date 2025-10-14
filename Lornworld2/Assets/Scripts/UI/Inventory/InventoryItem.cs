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
