using UnityEngine;

public abstract class BaseDropItemFeatureDeathBehavior<T> : FeatureDeathBehaviorScriptableObject<T> where T : FeatureData
{
    [SerializeField]
    private GameObject droppedItemPrefab;

    protected void DropItem(Vector2 pos, InventoryItem inventoryItem)
    {
        DroppedItem.Create(
            droppedItemPrefab,
            pos,
            inventoryItem.item,
            inventoryItem.stackSize);
    }
}
