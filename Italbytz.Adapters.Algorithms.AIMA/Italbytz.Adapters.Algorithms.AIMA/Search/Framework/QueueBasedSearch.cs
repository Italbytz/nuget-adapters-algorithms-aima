// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System;
using System.Collections.Generic;
using Italbytz.AI.Problem;
using Italbytz.AI.Search.Framework.QSearch;
using Italbytz.AI.Util.Datastructure;

namespace Italbytz.AI.Search.Framework;

public abstract class QueueBasedSearch<TState, TAction> :
    ISearchForActions<TState, TAction>, ISearchForStates<TState, TAction>
{
    private readonly NodePriorityQueue<TState, TAction> _frontier;

    private readonly QueueSearch<TState, TAction> Impl;

    protected QueueBasedSearch(QueueSearch<TState, TAction> impl,
        NodePriorityQueue<TState, TAction> queue)
    {
        Impl = impl;
        _frontier = queue;
    }

    public IMetrics Metrics => Impl.Metrics;

    public IEnumerable<TAction>? FindActions(
        IProblem<TState, TAction> problem)
    {
        Impl.NodeFactory.UseParentLinks = true;
        _frontier.Clear();
        var node = Impl.FindNode(problem, _frontier);
        return SearchUtils.ToActions(node);
    }

    public TState? FindState(IProblem<TState, TAction> problem)
    {
        throw new NotImplementedException();
    }
}