using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugItemUseBehavior", menuName = "Scriptable Objects/Items/UseBehaviors/Debug")]
public class DebugItemUseBehavior : ItemUseBehaviorScriptableObject
{
    public override void UseItem(Player player, Vector2 mousePos, RaycastHit2D raycastHit, Func<UnityEngine.Object, Vector3, Quaternion, InstantiateParameters, UnityEngine.Object> instantiate)
    {
        Debug.Log("used item");
    }
}
