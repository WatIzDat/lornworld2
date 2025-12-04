using MemoryPack;
using UnityEngine;

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

    public void UseItem(Player player, Vector2 mousePos, RaycastHit2D raycastHit, System.Func<Object, Vector3, Quaternion, InstantiateParameters, Object> instantiate)
    {
        if (item.itemUseBehavior != null)
        {
            item.itemUseBehavior.UseItem(player, mousePos, raycastHit, instantiate);
        }
    }
}

[MemoryPackable]
public partial record InventoryItemData : IGameData
{
    public ItemIdentifier item;
    public ItemScriptableObjectData itemData;
    public int stackSize;

    public InventoryItemData(ItemIdentifier item, int stackSize, ItemScriptableObjectData itemData = null)
    {
        this.item = item;
        this.itemData = itemData;
        this.stackSize = stackSize;
    }
}
