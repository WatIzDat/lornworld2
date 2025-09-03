using UnityEngine;

public class Player : Entity
{
    [SerializeField]
    private int hunger;

    protected override void OnDeath()
    {
        Debug.Log("death");
    }
}
