using UnityEngine.UIElements;

[UxmlElement]
public partial class CraftingRecipeUIElement : VisualElement
{
    //public InventorySlot[] RequiredItems { get; private set; }
    //public InventorySlot Result { get; private set; }

    public CraftingRecipeUIElement()
    {
        AddToClassList("crafting-recipe");
    }
    
    public void SetRecipe(InventoryItem[] items, InventoryItem result)
    {
        foreach (InventoryItem item in items)
        {
            InventorySlot inventorySlot = new()
            {
                isHotbarSlot = true
            };

            inventorySlot.SetItem(item);

            Add(inventorySlot);
        }

        InventorySlot resultSlot = new()
        {
            isHotbarSlot = true
        };

        resultSlot.AddToClassList("crafting-recipe-result");

        resultSlot.SetItem(result);

        Add(resultSlot);
    }
}
