public abstract class EnemyTransitionCondition : StateTransitionConditionScriptableObject
{
    protected Mob mob;

    public virtual void Initialize(Mob mob)
    {
        this.mob = mob;
    }
}
