// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System;
using System.Collections.Generic;
using Italbytz.AI.Problem;

namespace Italbytz.AI.Search.Framework;

public class NodeFactory<TState, TAction> : INodeFactory<TState, TAction>
{
    private readonly List<Action<INode<TState, TAction>>>
        _listeners = new();

    public bool UseParentLinks { get; set; } = true;

    public void AddNodeListener(Action<INode<TState, TAction>> listener)
    {
        _listeners.Add(listener);
    }

    public INode<TState, TAction> CreateNode(TState state)
    {
        return new Node<TState, TAction>(state);
    }

    public List<INode<TState, TAction>> GetSuccessors(
        INode<TState, TAction> node, IProblem<TState, TAction> problem)
    {
        var successors = new List<INode<TState, TAction>>();

        foreach (var action in problem.Actions(node.State))
        {
            var successorState = problem.Result(node.State, action);
            var stepCost =
                problem.StepCosts(node.State, action, successorState);
            successors.Add(CreateNode(successorState, node, action,
                stepCost));
        }

        NotifyListeners(node);
        return successors;
    }

    private INode<TState, TAction> CreateNode(TState state,
        INode<TState, TAction> parent, TAction action, double stepCost)
    {
        var p = UseParentLinks ? parent : null;
        return new Node<TState, TAction>(state, p, action,
            parent.PathCost + stepCost);
    }

    private void NotifyListeners(INode<TState, TAction> node)
    {
        _listeners.ForEach(listener => listener(node));
    }
}