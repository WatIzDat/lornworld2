using UnityEngine;

public class Mob : Entity
{
    // TODO: implement state machine and get rid of directly passing in player
    public static Mob Create(Player player, GameObject mobPrefab, Transform mobParent, MobScriptableObject mobScriptableObject, Vector2 position)
    {
        Mob mob = Instantiate(
            mobPrefab,
            position,
            Quaternion.identity,
            mobParent).GetComponent<Mob>();

        mob.maxHealth = mobScriptableObject.maxHealth;
        mob.health = mob.maxHealth;

        PathfindingUnit pathfindingUnit = mob.GetComponent<PathfindingUnit>();

        //Debug.Log(pathfindingUnit.);

        pathfindingUnit.target = player.transform;

        return mob;
    }
}
