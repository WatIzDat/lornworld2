using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "Scriptable Objects/Items/Item")]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public int maxStackSize;
    public ItemUseBehaviorScriptableObject itemUseBehavior;
    public ItemAttackBehaviorScriptableObject itemAttackBehavior;
    public ItemSelectBehaviorScriptableObject itemSelectBehavior;
    public StatScaleBehaviorScriptableObject statScaleBehavior;
    // TODO: make this into an item type enum
    public bool isArmor;
}
