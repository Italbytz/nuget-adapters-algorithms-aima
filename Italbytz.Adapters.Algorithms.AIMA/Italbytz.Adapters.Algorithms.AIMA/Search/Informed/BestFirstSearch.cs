// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System;
using Italbytz.AI.Search.Framework;
using Italbytz.AI.Search.Framework.QSearch;

namespace Italbytz.AI.Search.Informed;

public class BestFirstSearch<TState, TAction> :
    QueueBasedSearch<TState, TAction>, IInformed<TState, TAction>
{
    protected BestFirstSearch(QueueSearch<TState, TAction> impl,
        Func<INode<TState, TAction>, double> evalFn) : base(impl,
        QueueFactory.CreatePriorityQueue(evalFn))
    {
    }

    public Func<INode<TState, TAction>, double>? HeuristicFn { get; set; }
}