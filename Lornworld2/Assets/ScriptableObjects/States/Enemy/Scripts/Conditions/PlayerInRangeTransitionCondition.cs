using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInRangeTransitionCondition", menuName = "Scriptable Objects/States/Enemy/Conditions/PlayerInRange")]
public class PlayerInRangeTransitionCondition : EnemyTransitionCondition
{
    public float range;

    public override bool Condition()
    {
        return Vector2.Distance(mob.transform.position, mob.player.transform.position) < range;
    }
}
