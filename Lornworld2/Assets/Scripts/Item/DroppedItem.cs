using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public ItemScriptableObject Item { get; private set; }
    public int StackSize { get; private set; }

    public static DroppedItem Create(GameObject prefab, Vector2 pos, ItemScriptableObject item, int stackSize)
    {
        DroppedItem droppedItem = Instantiate(prefab, pos, Quaternion.identity).GetComponent<DroppedItem>();

        droppedItem.Item = item;
        droppedItem.StackSize = stackSize;

        droppedItem.GetComponent<SpriteRenderer>().sprite = item.sprite;

        return droppedItem;
    }
}
