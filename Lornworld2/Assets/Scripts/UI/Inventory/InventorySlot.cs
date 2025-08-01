using UnityEngine.UIElements;

[UxmlElement]
public partial class InventorySlot : VisualElement
{
    public Image icon;
    public ItemScriptableObject item;
    public Label stackSizeLabel;

    public InventorySlot()
    {
        icon = new Image();
        Add(icon);
        icon.AddToClassList("slot-icon");

        AddToClassList("inventory-slot");

        stackSizeLabel = new Label();
        Add(stackSizeLabel);
        stackSizeLabel.AddToClassList("stack-size-label");
    }
}
