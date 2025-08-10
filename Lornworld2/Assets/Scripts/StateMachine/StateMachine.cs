using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private IState currentState;

    private readonly Dictionary<Type, List<Transition>> transitionsMap = new();
    private readonly List<Transition> anyTransitions = new();
    private List<Transition> currentTransitions = new();

    private static readonly List<Transition> emptyTransitions = new(0);

    public void Tick()
    {
        Transition transition = GetTransition();

        if (transition != null)
        {
            SetState(transition.ToState);
        }

        currentState?.Tick();

        Debug.Log(currentState);
    }

    public void SetState(IState state)
    {
        if (state == currentState)
        {
            return;
        }

        currentState?.OnExit();
        currentState = state;

        transitionsMap.TryGetValue(currentState.GetType(), out currentTransitions);

        currentTransitions ??= emptyTransitions;

        currentState.OnEnter();
    }

    public void AddTransition(IState fromState, IState toState, Func<bool> predicate)
    {
        if (!transitionsMap.TryGetValue(fromState.GetType(), out List<Transition> transitions))
        {
            transitions = new List<Transition>();

            transitionsMap.Add(fromState.GetType(), transitions);
        }

        transitions.Add(new Transition(toState, predicate));
    }

    public void AddAnyTransition(IState toState, Func<bool> predicate)
    {
        anyTransitions.Add(new Transition(toState, predicate));
    }

    private Transition GetTransition()
    {
        foreach (Transition transition in anyTransitions)
        {
            if (transition.Condition())
            {
                return transition;
            }
        }

        foreach (Transition transition in currentTransitions)
        {
            if (transition.Condition())
            {
                return transition;
            }
        }

        return null;
    }

    private class Transition
    {
        public IState ToState { get; }
        public Func<bool> Condition { get; }

        public Transition(IState toState, Func<bool> condition)
        {
            ToState = toState;
            Condition = condition;
        }
    }
}
