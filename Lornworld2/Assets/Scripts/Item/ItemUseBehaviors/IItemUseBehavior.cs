using System;
using UnityEngine;

public interface IItemUseBehavior
{
    void UseItem(Player player, Vector2 mousePos, RaycastHit2D raycastHit, Func<UnityEngine.Object, Vector3, Quaternion, InstantiateParameters, UnityEngine.Object> instantiate);
}
