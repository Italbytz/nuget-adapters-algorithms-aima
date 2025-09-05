// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using Italbytz.AI.Search.Framework;
using Italbytz.AI.Search.Framework.QSearch;

namespace Italbytz.AI.Search.Uninformed;

public class
    UniformCostSearch<TState, TAction> : QueueBasedSearch<TState, TAction>
{
    public UniformCostSearch() : this(new GraphSearch<TState, TAction>())
    {
    }

    private UniformCostSearch(QueueSearch<TState, TAction> impl) : base(
        impl,
        QueueFactory.CreatePriorityQueue<TState, TAction>(node =>
            node.PathCost))

    {
    }
}