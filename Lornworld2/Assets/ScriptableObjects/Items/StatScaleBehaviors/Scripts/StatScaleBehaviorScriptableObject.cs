using UnityEngine;

public abstract class StatScaleBehaviorScriptableObject : ScriptableObject, IStatScaleBehavior
{
    public abstract StatScaleInfo GetStatScaleInfo();
}
