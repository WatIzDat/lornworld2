public record StatScaleInfo
{
    public float AttackDamageScaler { get; }
    public float HealthScaler { get; }

    public StatScaleInfo(float attackDamageScaler = 1f, float healthScaler = 1f)
    {
        AttackDamageScaler = attackDamageScaler;
        HealthScaler = healthScaler;
    }
}
