using MemoryPack;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "Scriptable Objects/Items/Item")]
public class ItemScriptableObject : RegistryEntry
{
    public Sprite sprite;
    public int maxStackSize;
    public ItemUseBehaviorScriptableObject itemUseBehavior;
    public ItemAttackBehaviorScriptableObject itemAttackBehavior;
    public ItemSelectBehaviorScriptableObject itemSelectBehavior;
    public StatScaleBehaviorScriptableObject statScaleBehavior;
    // TODO: make this into an item type enum
    public bool isArmor;
    public bool isWeapon;

    public bool makeUniqueRuntimeInstances;

    public void SetItemAttackBehavior(ItemAttackBehaviorScriptableObject itemAttackBehavior)
    {
        if (!makeUniqueRuntimeInstances)
        {
            Debug.LogError("Not a unique runtime instance");

            return;
        }

        this.itemAttackBehavior = itemAttackBehavior;
    }
}

[MemoryPackable]
public partial class ItemScriptableObjectData : IGameData
{
    public string itemAttackBehaviorAssetPath;
}
