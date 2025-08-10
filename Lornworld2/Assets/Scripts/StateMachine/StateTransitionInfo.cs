using System;

[Serializable]
public class StateTransitionInfo<T> where T : StateScriptableObject
{
    public T fromState;
    public T toState;
    public StateTransitionCondition condition;
}
