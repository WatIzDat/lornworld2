using UnityEngine;

public abstract class BaseFeatureHitBehaviorScriptableObject : ScriptableObject, IFeatureHitBehavior<FeatureData>
{
    public abstract void Hit(FeatureData data);
}

public abstract class FeatureHitBehaviorScriptableObject<T> : BaseFeatureHitBehaviorScriptableObject where T : FeatureData
{
    public override void Hit(FeatureData data)
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
