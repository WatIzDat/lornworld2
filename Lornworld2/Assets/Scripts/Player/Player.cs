using UnityEngine;

public class Player : Entity
{
    public float baseAttackDamage;

    [SerializeField]
    private int hunger;

    protected override void OnDeath()
    {
        Debug.Log("death");
    }
}
