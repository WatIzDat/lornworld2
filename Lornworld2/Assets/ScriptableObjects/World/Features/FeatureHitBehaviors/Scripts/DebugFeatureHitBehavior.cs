using UnityEngine;

[CreateAssetMenu(fileName = "DebugFeatureHitBehavior", menuName = "Scriptable Objects/World/Features/HitBehaviors/Debug")]
public class DebugFeatureHitBehavior : FeatureHitBehaviorScriptableObject<EmptyFeatureData>
{
    public override void Hit(FeatureData data)
    {
        base.Hit(data);

        Debug.Log("hit");
    }
}
