public record InventoryItem
{
    public ItemScriptableObject Item { get; }

    public int StackSize { get; }

    public InventoryItem(ItemScriptableObject item, int stackSize = 1)
    {
        Item = item;
        StackSize = stackSize;
    }

    public InventoryItem AddStack(int stack)
    {
        return new InventoryItem(Item, StackSize + stack);
    }

    public InventoryItem WithStack(int stack)
    {
        return new InventoryItem(Item, stack);
    }

    public void UseItem()
    {
        if (Item.itemUseBehavior != null)
        {
            Item.itemUseBehavior.UseItem();
        }
    }
}
