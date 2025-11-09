using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FeatureItemUseBehavior", menuName = "Scriptable Objects/Items/UseBehaviors/Feature")]
public class FeatureItemUseBehavior : ItemUseBehaviorScriptableObject
{
    public event Action<FeatureScriptableObject> ItemUsed;

    public FeatureScriptableObject feature;

    public override void UseItem()
    {
        ItemUsed?.Invoke(feature);
    }
}
