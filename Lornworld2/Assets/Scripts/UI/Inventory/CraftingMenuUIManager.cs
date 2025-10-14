using UnityEngine;
using UnityEngine.UIElements;

public class CraftingMenuUIManager : MonoBehaviour
{
    private VisualElement root;
    private ScrollView recipeContainer;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        recipeContainer = root.Q<ScrollView>("RecipeContainer");
    }

    private void Start()
    {
        CraftingRecipeUIElement craftingRecipe = new();
        craftingRecipe.SetRecipe(new InventoryItem[] { new(ItemRegistry.Instance.GetEntry(ItemIds.GrassItem)), new(ItemRegistry.Instance.GetEntry(ItemIds.GrassItem), 5) }, new(ItemRegistry.Instance.GetEntry(ItemIds.StoneItem)));

        recipeContainer.Add(craftingRecipe);
    }
}
