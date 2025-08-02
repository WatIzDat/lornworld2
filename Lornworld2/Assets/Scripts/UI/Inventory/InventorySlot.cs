using UnityEngine.UIElements;

[UxmlElement]
public partial class InventorySlot : VisualElement
{
    public Image Icon { get; private set; }
    public InventoryItem InventoryItem { get; private set; }
    public Label StackSizeLabel { get; private set; }

    public int index;
    public InventoryUIManager inventoryUIManager;

    public bool IsEmpty => InventoryItem == null;

    public InventorySlot()
    {
        Icon = new Image();
        Add(Icon);
        Icon.AddToClassList("slot-icon");

        AddToClassList("inventory-slot");

        StackSizeLabel = new Label();
        Add(StackSizeLabel);
        StackSizeLabel.AddToClassList("stack-size-label");

        RegisterCallback<PointerDownEvent>(OnPointerDown);
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        if (InventoryItem == null)
        {
            return;
        }

        //icon.image = null;

        if (evt.button == 0)
        {
            inventoryUIManager.StartDrag(evt.position, this, InventoryItem.StackSize);
        }
        else if (evt.button == 1)
        {
            inventoryUIManager.StartDrag(evt.position, this, InventoryItem.StackSize / 2);
        }
    }

    public void SetItem(InventoryItem inventoryItem)
    {
        InventoryItem = inventoryItem;
        Icon.image = inventoryItem.Item.sprite.texture;
        StackSizeLabel.text = inventoryItem.StackSize.ToString();
    }

    public void DropItem()
    {
        InventoryItem = null;
        Icon.image = null;
        StackSizeLabel.text = null;
    }

    //public bool CanItemBeSet(InventoryItem item)
    //{
    //    if (IsEmpty)
    //    {
    //        return true;
    //    }

    //    bool isSameItem = inventoryItem.Item == item.Item;
    //    bool isNotOverflowing = inventoryItem.StackSize + item.StackSize <= inventoryItem.Item.maxStackSize;

    //    return isSameItem && isNotOverflowing;
    //}
}
