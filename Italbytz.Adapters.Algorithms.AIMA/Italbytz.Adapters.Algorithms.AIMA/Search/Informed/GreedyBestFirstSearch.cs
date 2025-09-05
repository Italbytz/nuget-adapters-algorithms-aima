using System;
using Italbytz.AI.Search;
using Italbytz.AI.Search.Framework;
using Italbytz.AI.Search.Framework.QSearch;
using Italbytz.AI.Search.Informed;

namespace Italbytz.Adapters.Algorithms.AI.Search.Informed;

public class GreedyBestFirstSearch<TState, TAction> :
    QueueBasedSearch<TState, TAction>, IInformed<TState, TAction>
{
    public GreedyBestFirstSearch(QueueSearch<TState, TAction> impl,
        Func<INode<TState, TAction>, double> evalFn) : base(impl,
        QueueFactory.CreatePriorityQueue(evalFn))
    {
    }

    public Func<INode<TState, TAction>, double>? HeuristicFn { get; set; }
}