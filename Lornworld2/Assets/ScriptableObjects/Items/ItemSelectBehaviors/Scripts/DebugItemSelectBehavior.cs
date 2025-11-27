using UnityEngine;

[CreateAssetMenu(fileName = "DebugItemSelectBehavior", menuName = "Scriptable Objects/Items/SelectBehaviors/Debug")]
public class DebugItemSelectBehavior : ItemSelectBehaviorScriptableObject
{
    public override void SelectItem()
    {
        Debug.Log("item selected");
    }

    public override void DeselectItem()
    {
        Debug.Log("item deselected");
    }
}