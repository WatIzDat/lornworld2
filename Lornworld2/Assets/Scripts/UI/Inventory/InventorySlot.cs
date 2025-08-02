using UnityEngine.UIElements;

[UxmlElement]
public partial class InventorySlot : VisualElement
{
    public Image icon;
    public InventoryItem item;
    public Label stackSizeLabel;
    public int index;

    public bool IsEmpty => item == null;

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
        if (evt.button != 0 || item == null)
        {
            return;
        }

        icon.image = null;

        InventoryUIManager.StartDrag(evt.position, this);
    }

    public void SetItem(InventoryItem inventoryItem)
    {
        item = inventoryItem;
        icon.image = inventoryItem.Item.sprite.texture;
        stackSizeLabel.text = inventoryItem.StackSize.ToString();
    }

    public void DropItem()
    {
        item = null;
        icon.image = null;
        stackSizeLabel.text = null;
    }
}
