using UnityEngine;

[CreateAssetMenu(fileName = "SlimeMobFollowPathBehavior", menuName = "Scriptable Objects/Mobs/FollowPathBehaviors/Slime")]
public class SlimeMobFollowPathBehavior : MobFollowPathBehaviorScriptableObject
{
    public override void FollowPath(Rigidbody2D rb, float speed, float time, Vector2 direction, Vector2 startPos, Vector2 targetPos)
    {
        rb.linearVelocity = speed * ((Mathf.Sin(5f * time) + 1) / 2) * Time.fixedDeltaTime * direction;
    }
}
