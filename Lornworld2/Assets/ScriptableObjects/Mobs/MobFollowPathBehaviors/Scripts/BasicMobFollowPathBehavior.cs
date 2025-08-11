using UnityEngine;

[CreateAssetMenu(fileName = "BasicMobFollowPathBehavior", menuName = "Scriptable Objects/Mobs/FollowPathBehaviors/Basic")]
public class BasicMobFollowPathBehavior : MobFollowPathBehaviorScriptableObject
{
    public override void FollowPath(Rigidbody2D rb, float speed, Vector2 direction, Vector2 startPos, Vector2 targetPos)
    {
        rb.linearVelocity = speed * Time.fixedDeltaTime * direction;
    }
}
