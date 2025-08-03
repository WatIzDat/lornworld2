using System;
using UnityEngine;

public class ItemRegistry : Registry<ItemScriptableObject, ItemIdentifier>
{
    public static ItemRegistry Instance { get; private set; }

    protected override Type IdsType { get; } = typeof(ItemIds);

    protected override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);

            Debug.LogError("Can't create another ItemRegistry instance");
        }
        else
        {
            Instance = this;
        }

        base.Awake();
    }

    protected override ItemIdentifier CreateId(string id)
    {
        return new ItemIdentifier(id);
    }
}
