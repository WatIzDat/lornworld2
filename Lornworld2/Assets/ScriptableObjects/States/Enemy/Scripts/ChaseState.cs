using UnityEngine;

[CreateAssetMenu(fileName = "ChaseState", menuName = "Scriptable Objects/States/Enemy/Chase")]
public class ChaseState : EnemyState
{
    private Entity target;
    private PathfindingUnit pathfindingUnit;

    //public ChaseState(Transform target, PathfindingUnit pathfindingUnit)
    //{
    //    this.target = target;
    //    this.pathfindingUnit = pathfindingUnit;
    //}

    public override void Initialize(Mob mob)
    {
        base.Initialize(mob);

        target = mob.player.GetComponent<Entity>();
        pathfindingUnit = mob.GetComponent<PathfindingUnit>();
    }

    public override void OnEnter()
    {
        pathfindingUnit.RequestFollowPath(target.transform);
    }

    public override void OnExit()
    {
        pathfindingUnit.StopFollowPath();
    }

    public override void Tick()
    {
        if (target.MovedThisFrame)
        {
            Debug.Log("moved");

            pathfindingUnit.RequestFollowPath(target.transform);
        }
    }
}
