using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobScriptableObject", menuName = "Scriptable Objects/Mobs/Mob")]
public class MobScriptableObject : ScriptableObject
{
    public string mobName;
    public float maxHealth;
    public MobPathfindingBehaviorScriptableObject mobPathfindingBehavior;
    public MobFollowPathBehaviorScriptableObject mobFollowPathBehavior;
    public List<StateTransitionInfo<EnemyState, EnemyTransitionCondition>> stateTransitions;
    public EnemyState initialState;
}
