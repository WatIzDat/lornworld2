using System;

[Serializable]
public class StateTransitionInfo<T, U> where T : StateScriptableObject where U : StateTransitionConditionScriptableObject
{
    public T fromState;
    public T toState;
    public U condition;
}
