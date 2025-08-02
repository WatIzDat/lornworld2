using UnityEngine.UIElements;

[UxmlElement]
public partial class InventorySlot : VisualElement
{
    public Image icon;
    public InventoryItem inventoryItem;
    public Label stackSizeLabel;
    public int index;

    public bool IsEmpty => inventoryItem == null;

    public InventorySlot()
    {
        icon = new Image();
        Add(icon);
        icon.AddToClassList("slot-icon");

        AddToClassList("inventory-slot");

        stackSizeLabel = new Label();
        Add(stackSizeLabel);
        stackSizeLabel.AddToClassList("stack-size-label");

        RegisterCallback<PointerDownEvent>(OnPointerDown);
    }

    private void OnPointerDown(PointerDownEvent evt)
    {
        if (evt.button != 0 || inventoryItem == null)
        {
            return;
        }

        icon.image = null;

        InventoryUIManager.StartDrag(evt.position, this);
    }

    public void SetItem(InventoryItem inventoryItem)
    {
        this.inventoryItem = inventoryItem;
        icon.image = inventoryItem.Item.sprite.texture;
        stackSizeLabel.text = inventoryItem.StackSize.ToString();
    }

    public void DropItem()
    {
        inventoryItem = null;
        icon.image = null;
        stackSizeLabel.text = null;
    }

    public bool CanItemBeSet(InventoryItem item)
    {
        if (IsEmpty)
        {
            return true;
        }

        bool isSameItem = inventoryItem.Item == item.Item;
        bool isNotOverflowing = inventoryItem.StackSize + item.StackSize <= inventoryItem.Item.maxStackSize;

        return isSameItem && isNotOverflowing;
    }
}
