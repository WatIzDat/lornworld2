using UnityEngine;
using UnityEngine.UIElements;

public class CraftingMenuUIManager : MonoBehaviour
{
    [SerializeField]
    private InventoryUIManager inventoryUIManager;

    private VisualElement root;
    private ScrollView recipeContainer;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        recipeContainer = root.Q<ScrollView>("RecipeContainer");
    }

    private void Start()
    {
        foreach (CraftingRecipeScriptableObject craftingRecipe in CraftingRecipeRegistry.Instance.Entries)
        {
            CraftingRecipeUIElement craftingRecipeUIElement = new(() =>
            {
                foreach (InventoryItem item in craftingRecipe.items)
                {
                    if (!inventoryUIManager.ContainsItem(item.item, item.stackSize))
                    {
                        return;
                    }
                }

                foreach (InventoryItem item in craftingRecipe.items)
                {
                    inventoryUIManager.RemoveItem(item.item, item.stackSize);
                }

                inventoryUIManager.AddItem(craftingRecipe.result.item, craftingRecipe.result.stackSize);
            });

            craftingRecipeUIElement.SetRecipe(craftingRecipe);

            recipeContainer.Add(craftingRecipeUIElement);
        }
    }
}
