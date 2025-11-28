using UnityEngine;

public abstract class BaseFeatureDeathBehaviorScriptableObject : ScriptableObject, IFeatureDeathBehavior<FeatureData>
{
    public abstract void Die(FeatureData data, Vector2 deathPos);
}

public abstract class FeatureDeathBehaviorScriptableObject<T> : BaseFeatureDeathBehaviorScriptableObject where T : FeatureData
{
    public override void Die(FeatureData data, Vector2 deathPos)
    {
        if (data == null)
        {
            return;
        }

        if (data.GetType() != typeof(T))
        {
            Debug.LogError("Can't convert types");

            return;
        }
    }
}
