using UnityEngine;

public abstract class ItemSelectBehaviorScriptableObject : ScriptableObject, IItemSelectBehavior
{
    public abstract void SelectItem();
    public abstract void DeselectItem();
}
