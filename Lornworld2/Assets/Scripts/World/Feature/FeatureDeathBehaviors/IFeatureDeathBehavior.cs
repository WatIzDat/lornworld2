using UnityEngine;

public interface IFeatureDeathBehavior<in T> where T : FeatureData
{
    void Die(T data, Vector2 deathPos);
}
