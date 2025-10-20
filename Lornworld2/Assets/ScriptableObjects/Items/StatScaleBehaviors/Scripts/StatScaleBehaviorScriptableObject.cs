using UnityEngine;

[CreateAssetMenu(fileName = "StatScaleBehaviorScriptableObject", menuName = "Scriptable Objects/Items/StatScaleBehaviors/Default")]
public class StatScaleBehaviorScriptableObject : ScriptableObject, IStatScaleBehavior
{
    public float attackDamageScaler = 1f;
    public float healthScaler = 1f;

    public virtual StatScaleInfo GetStatScaleInfo()
    {
        return new StatScaleInfo(attackDamageScaler, healthScaler);
    }
}
