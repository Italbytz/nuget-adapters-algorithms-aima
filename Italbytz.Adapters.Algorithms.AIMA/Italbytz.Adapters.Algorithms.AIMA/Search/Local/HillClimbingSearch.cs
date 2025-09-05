// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System;
using System.Collections.Generic;
using Italbytz.AI.Problem;
using Italbytz.AI.Search.Framework;
using Italbytz.AI.Util;
using Microsoft.Extensions.Logging;

namespace Italbytz.AI.Search.Local;

public class HillClimbingSearch<TState, TAction> :
    ISearchForActions<TState, TAction>, ISearchForStates<TState, TAction>
{
    public const string MetricNodesExpanded = "nodesExpanded";
    public const string MetricNodeValue = "nodeValue";
    private readonly Func<INode<TState, TAction>, double> _evalFn;
    private readonly INodeFactory<TState, TAction> _nodeFactory;
    private TState? _lastState;

    public HillClimbingSearch(Func<INode<TState, TAction>, double> evalFn) :
        this(evalFn, new NodeFactory<TState, TAction>())
    {
    }

    public HillClimbingSearch(Func<INode<TState, TAction>, double> evalFn,
        INodeFactory<TState, TAction> nodeFactory)
    {
        _evalFn = evalFn;
        _nodeFactory = nodeFactory;
        nodeFactory.AddNodeListener(node =>
            Metrics.IncrementInt(MetricNodesExpanded));
    }

    public IMetrics Metrics { get; } = new Metrics();

    public IEnumerable<TAction>? FindActions(
        IProblem<TState, TAction> problem)
    {
        _nodeFactory.UseParentLinks = true;
        return SearchUtils.ToActions(FindNode(problem));
    }

    public TState? FindState(IProblem<TState, TAction> problem)
    {
        throw new NotImplementedException();
    }

    private INode<TState, TAction>? FindNode(IProblem<TState, TAction> p)
    {
        ClearMetrics();
        var current = _nodeFactory.CreateNode(p.InitialState);
        this.Log(LogLevel.Information, current.State.ToString());
        while (true)
        {
            Metrics.Set(MetricNodeValue, _evalFn(current));
            var neighbor =
                GetHighestValuedNodeFrom(
                    _nodeFactory.GetSuccessors(current, p));
            this.Log(LogLevel.Information, neighbor.State.ToString());
            if (neighbor == null || _evalFn(neighbor) <= _evalFn(current))
            {
                _lastState = current.State;
                return p.TestSolution(current) ? current : null;
            }

            current = neighbor;
        }

        _lastState = current.State;
        return null;
    }

    private void ClearMetrics()
    {
        Metrics.Set(MetricNodesExpanded, 0);
        Metrics.Set(MetricNodeValue, 0);
    }

    private INode<TState, TAction> GetHighestValuedNodeFrom(
        List<INode<TState, TAction>> children)
    {
        var highestValue = double.NegativeInfinity;
        INode<TState, TAction>? nodeWithHighestValue = null;
        foreach (var child in children)
        {
            var value = _evalFn(child);
            if (value > highestValue)
            {
                highestValue = value;
                nodeWithHighestValue = child;
            }
        }

        return nodeWithHighestValue!;
    }
}