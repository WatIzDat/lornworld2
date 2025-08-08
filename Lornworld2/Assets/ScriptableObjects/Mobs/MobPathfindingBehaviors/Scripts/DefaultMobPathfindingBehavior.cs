using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultMobPathfindingBehavior", menuName = "Scriptable Objects/Mobs/PathfindingBehaviors/Default")]
public class DefaultMobPathfindingBehavior : MobPathfindingBehaviorScriptableObject
{
    public override void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback)
    {
        PathRequestManager.RequestPath(pathStart, pathEnd, callback);
    }
}
