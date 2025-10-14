using System;
using UnityEngine;

public class CraftingRecipeRegistry : Registry<CraftingRecipeScriptableObject, CraftingRecipeIdentifier>
{
    public static CraftingRecipeRegistry Instance { get; private set; }

    protected override Type IdsType { get; } = typeof(CraftingRecipeIds);

    protected override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

            Debug.LogError("Can't create another CraftingRecipeRegistry instance");
        }
        else
        {
            Instance = this;
        }

        base.Awake();
    }

    protected override CraftingRecipeIdentifier CreateId(string id)
    {
        return new CraftingRecipeIdentifier(id);
    }
}
