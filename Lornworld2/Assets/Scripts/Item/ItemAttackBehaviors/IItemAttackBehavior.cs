using UnityEngine;

public interface IItemAttackBehavior
{
    void Attack(RaycastHit2D raycastHit, bool isEntity, Entity entity, System.Func<Object, Transform, Object> instantiate);
}
