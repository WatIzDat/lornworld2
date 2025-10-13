using UnityEngine;

[CreateAssetMenu(fileName = "DebugStatScaleBehavior", menuName = "Scriptable Objects/Items/StatScaleBehaviors/Debug")]
public class DebugStatScaleBehavior : StatScaleBehaviorScriptableObject
{
    public override StatScaleInfo GetStatScaleInfo()
    {
        return new StatScaleInfo(attackDamageScaler: 2f);
    }
}
