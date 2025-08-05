using UnityEngine;

public abstract class ItemUseBehaviorScriptableObject : ScriptableObject, IItemUseBehavior
{
    public abstract void UseItem();
}
