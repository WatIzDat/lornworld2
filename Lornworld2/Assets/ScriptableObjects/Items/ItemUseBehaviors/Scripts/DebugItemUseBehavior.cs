using UnityEngine;

[CreateAssetMenu(fileName = "DebugItemUseBehavior", menuName = "Scriptable Objects/Items/UseBehaviors/Debug")]
public class DebugItemUseBehavior : ItemUseBehaviorScriptableObject
{
    public override void UseItem()
    {
        Debug.Log("used item");
    }
}
