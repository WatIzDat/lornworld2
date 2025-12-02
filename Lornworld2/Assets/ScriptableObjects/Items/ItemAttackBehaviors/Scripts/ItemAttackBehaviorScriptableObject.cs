using UnityEngine;

public abstract class ItemAttackBehaviorScriptableObject : ScriptableObject, IItemAttackBehavior
{
    public abstract void Attack(RaycastHit2D raycastHit, bool isEntity, Entity entity, System.Func<Object, Transform, Object> instantiate);
}
