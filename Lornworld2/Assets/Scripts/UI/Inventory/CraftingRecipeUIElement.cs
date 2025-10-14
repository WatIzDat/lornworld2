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
    
    public void SetRecipe(CraftingRecipeScriptableObject craftingRecipe)
    {
        foreach (InventoryItem item in craftingRecipe.items)
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

        resultSlot.SetItem(craftingRecipe.result);

        Add(resultSlot);
    }
}
