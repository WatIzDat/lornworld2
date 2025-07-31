using UnityEngine.UIElements;

[UxmlElement]
public partial class InventorySlot : VisualElement
{
    public Image icon;
    public string itemId;

    public InventorySlot()
    {
        icon = new Image();
        Add(icon);

        icon.AddToClassList("slot-icon");
        AddToClassList("inventory-slot");
    }
}
