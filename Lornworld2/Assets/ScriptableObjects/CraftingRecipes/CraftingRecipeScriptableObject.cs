using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipeScriptableObject", menuName = "Scriptable Objects/CraftingRecipes/CraftingRecipe")]
public class CraftingRecipeScriptableObject : ScriptableObject
{
    public InventoryItem[] items;
    public InventoryItem result;
}
