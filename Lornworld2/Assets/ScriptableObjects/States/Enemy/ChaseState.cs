using UnityEngine;

public class ChaseState : EnemyState
{
    private Transform target;
    private PathfindingUnit pathfindingUnit;

    //public ChaseState(Transform target, PathfindingUnit pathfindingUnit)
    //{
    //    this.target = target;
    //    this.pathfindingUnit = pathfindingUnit;
    //}

    public override void Initialize(Mob mob)
    {
        base.Initialize(mob);

        target = mob.player.transform;
        pathfindingUnit = mob.GetComponent<PathfindingUnit>();
    }

    public override void OnEnter()
    {
        pathfindingUnit.RequestFollowPath(target);
    }

    public override void OnExit()
    {
        pathfindingUnit.StopFollowPath();
    }

    public override void Tick()
    {
    }
}
