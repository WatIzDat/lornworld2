using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FeatureItemUseBehavior", menuName = "Scriptable Objects/Items/UseBehaviors/Feature")]
public class FeatureItemUseBehavior : ItemUseBehaviorScriptableObject
{
    public event Action<FeatureScriptableObject> ItemUsed;

    public FeatureScriptableObject feature;

    public override void UseItem(Player player, Vector2 mousePos, RaycastHit2D raycastHit, Func<UnityEngine.Object, Vector3, Quaternion, InstantiateParameters, UnityEngine.Object> instantiate)
    {
        ItemUsed?.Invoke(feature);
    }
}
