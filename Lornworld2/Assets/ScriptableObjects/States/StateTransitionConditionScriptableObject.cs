using UnityEngine;

public abstract class StateTransitionConditionScriptableObject : ScriptableObject
{
    public abstract bool Condition();

    public bool InverseCondition()
    {
        return !Condition();
    }
}
