public record StatScaleInfo
{
    public float AttackDamageScaler { get; }

    public StatScaleInfo(float attackDamageScaler = 1f)
    {
        AttackDamageScaler = attackDamageScaler;
    }
}
