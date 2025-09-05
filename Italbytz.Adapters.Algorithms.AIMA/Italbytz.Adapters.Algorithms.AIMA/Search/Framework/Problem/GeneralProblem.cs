// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System;
using System.Collections.Generic;
using Italbytz.AI.Problem;

namespace Italbytz.AI.Search.Framework.Problem;

/// <inheritdoc cref="IProblem{TState,TAction}" />
public class GeneralProblem<TState, TAction> : IProblem<TState, TAction>
{
    public GeneralProblem(TState initialState,
        Func<TState, List<TAction>> actions,
        Func<TState, TAction, TState> result,
        Func<TState, bool> goalTest,
        Func<TState, TAction, TState, double> stepCosts)
    {
        InitialState = initialState;
        Actions = actions;
        Result = result;
        GoalTest = goalTest;
        StepCosts = stepCosts;
    }

    public GeneralProblem(TState initialState,
        Func<TState, List<TAction>> actions,
        Func<TState, TAction, TState> result,
        Func<TState, bool> goalTest) : this(initialState, actions, result,
        goalTest, (s, a, sPrimed) => 1.0)
    {
    }

    public TState InitialState { get; }

    public Func<TState, TAction, TState> Result { get; }

    public Func<TState, List<TAction>> Actions { get; }

    public Func<TState, bool> GoalTest { get; }

    public Func<TState, TAction, TState, double> StepCosts { get; }

    public bool TestSolution(INode<TState, TAction> node)
    {
        return GoalTest(node.State);
    }

    public double GetStepCosts(TState state, TAction action, TState stateDelta)
    {
        throw new NotImplementedException();
    }
}