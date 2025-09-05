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

public class SimulatedAnnealingSearch<TState, TAction> :
    ISearchForActions<TState, TAction>, ISearchForStates<TState, TAction>
{
    public const string MetricNodesExpanded = "nodesExpanded";
    public const string MetricNodeValue = "nodeValue";
    public const string MetricTemperature = "temp";

    private readonly Func<INode<TState, TAction>, double> _energyFn;
    private readonly INodeFactory<TState, TAction> _nodeFactory;
    private readonly Scheduler _scheduler;
    private TState? _lastState;

    public SimulatedAnnealingSearch(
        Func<INode<TState, TAction>, double> energyFn) : this(energyFn,
        new Scheduler())
    {
    }

    public SimulatedAnnealingSearch(
        Func<INode<TState, TAction>, double> energyFn,
        Scheduler scheduler) : this(energyFn, scheduler,
        new NodeFactory<TState, TAction>())
    {
    }

    public SimulatedAnnealingSearch(
        Func<INode<TState, TAction>, double> energyFn, Scheduler scheduler,
        INodeFactory<TState, TAction> nodeFactory)
    {
        _energyFn = energyFn;
        _scheduler = scheduler;
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
        var timeStep = 0;
        var random = ThreadSafeRandomNetCore.LocalRandom;
        while (true)
        {
            var temperature = _scheduler.GetTemp(timeStep++);
            _lastState = current.State;
            if (temperature == 0.0)
                return p.TestSolution(current) ? current : null;
            Metrics.Set(MetricTemperature, temperature);
            Metrics.Set(MetricNodeValue, _energyFn(current));
            var children = _nodeFactory.GetSuccessors(current, p);
            if (children.Count > 0)
            {
                var next = Util.Util.SelectRandomlyFromList(children);
                this.Log(LogLevel.Information, next.State.ToString());
                var deltaE = _energyFn(next) - _energyFn(current);
                if (deltaE < 0.0 || random.NextDouble() <=
                    Math.Exp(-deltaE / temperature)) current = next;
            }
            else
            {
                return null;
            }
        }

        _lastState = current.State;
        return null;
    }

    private void ClearMetrics()
    {
        Metrics.Set(MetricNodesExpanded, 0);
        Metrics.Set(MetricNodeValue, 0);
        Metrics.Set(MetricTemperature, 0);
    }

    private INode<TState, TAction> GetHighestValuedNodeFrom(
        List<INode<TState, TAction>> children)
    {
        var highestValue = double.NegativeInfinity;
        INode<TState, TAction>? nodeWithHighestValue = null;
        foreach (var child in children)
        {
            var value = _energyFn(child);
            if (value > highestValue)
            {
                highestValue = value;
                nodeWithHighestValue = child;
            }
        }

        return nodeWithHighestValue!;
    }
}