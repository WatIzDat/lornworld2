using UnityEngine;

public abstract class MobFollowPathBehaviorScriptableObject : ScriptableObject, IMobFollowPathBehavior
{
    public abstract void FollowPath(Rigidbody2D rb, float speed, Vector2 direction, Vector2 startPos, Vector2 targetPos);
}
