using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "Scriptable Objects/Items/Item")]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public int maxStackSize;
    public ItemUseBehaviorScriptableObject itemUseBehavior;
    // TODO: Implement stat scale behavior that returns a stat scale object maybe
}
