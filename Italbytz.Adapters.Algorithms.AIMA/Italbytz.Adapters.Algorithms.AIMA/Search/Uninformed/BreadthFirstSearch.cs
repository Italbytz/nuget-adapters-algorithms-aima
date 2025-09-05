using Italbytz.AI.Search.Framework;
using Italbytz.AI.Search.Framework.QSearch;

namespace Italbytz.Adapters.Algorithms.AI.Search.Uninformed;

public class
    BreadthFirstSearch<TState, TAction> : QueueBasedSearch<TState, TAction>
{
    public BreadthFirstSearch() : this(new TreeSearch<TState, TAction>())
    {
    }

    private BreadthFirstSearch(QueueSearch<TState, TAction> impl) : base(
        impl,
        QueueFactory.CreatePriorityQueue<TState, TAction>(node =>
            node.PathCost))

    {
    }
}