using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInChaseRangeTransitionCondition", menuName = "Scriptable Objects/States/Enemy/Conditions/PlayerInChaseRange")]
public class PlayerInChaseRangeTransitionCondition : EnemyTransitionCondition
{
    public float chaseRange;

    public override bool Condition()
    {
        return Vector2.Distance(mob.transform.position, mob.player.transform.position) < chaseRange;
    }
}
