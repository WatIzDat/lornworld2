using UnityEngine;

public abstract class StateScriptableObject : ScriptableObject, IState
{
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void Tick();
}
