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
        foreach (CraftingRecipeScriptableObject craftingRecipe in CraftingRecipeRegistry.Instance.Entries)
        {
            CraftingRecipeUIElement craftingRecipeUIElement = new();

            craftingRecipeUIElement.SetRecipe(craftingRecipe);

            recipeContainer.Add(craftingRecipeUIElement);
        }
    }
}
