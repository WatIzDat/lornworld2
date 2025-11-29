using UnityEngine;

[CreateAssetMenu(fileName = "DebugFeatureHitBehavior", menuName = "Scriptable Objects/World/Features/HitBehaviors/Debug")]
public class DebugFeatureHitBehavior : FeatureHitBehaviorScriptableObject<EmptyFeatureData>
{
    public override void Hit(FeatureData data, Vector2 pos)
    {
        base.Hit(data, pos);

        Debug.Log("hit");
    }
}
