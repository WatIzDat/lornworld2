public abstract class EnemyState : StateScriptableObject
{
    protected Mob mob;

    public virtual void Initialize(Mob mob)
    {
        this.mob = mob;
    }
}
