using System;
using UnityEngine;

public abstract class ItemUseBehaviorScriptableObject : ScriptableObject, IItemUseBehavior
{
    public abstract void UseItem(Player player, Vector2 mousePos, RaycastHit2D raycastHit, Func<UnityEngine.Object, Vector3, Quaternion, InstantiateParameters, UnityEngine.Object> instantiate);
}
