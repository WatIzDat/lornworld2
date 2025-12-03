using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipeScriptableObject", menuName = "Scriptable Objects/CraftingRecipes/CraftingRecipe")]
public class CraftingRecipeScriptableObject : RegistryEntry
{
    public InventoryItem[] items;
    public InventoryItem result;
}
