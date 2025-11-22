using UnityEngine;

public abstract class BaseFeatureInitBehaviorScriptableObject : ScriptableObject, IFeatureInitBehavior<FeatureData>
{
    //public virtual Feature Init(Feature feature, FeatureData data)
    //{
    //    if (data.GetType() != typeof(T))
    //    {
    //        Debug.LogError("Can't convert types");

    //        return null;
    //    }

    //    return feature;
    //}

    public abstract Feature Init(Feature feature, FeatureData data);
}

public abstract class FeatureInitBehaviorScriptableObject<T> : BaseFeatureInitBehaviorScriptableObject where T : FeatureData
{
    public override Feature Init(Feature feature, FeatureData data)
    {
        if (data.GetType() != typeof(T))
        {
            Debug.LogError("Can't convert types");

            return null;
        }

        feature.data = data;

        return feature;
    }
}
