using System;
using System.Collections.Generic;
using Italbytz.AI.Search;

namespace Italbytz.AI.Util.Datastructure;

public class
    NodePriorityQueue<TState, TAction> : PriorityQueue<
    INode<TState, TAction>, double>
{
    private readonly Func<INode<TState, TAction>, double> _priorityFn;

    public NodePriorityQueue(
        Func<INode<TState, TAction>, double> priorityFn,
        int initialCapacity) : base(initialCapacity)
    {
        _priorityFn = priorityFn;
    }

    public void Enqueue(INode<TState, TAction> element)
    {
        base.Enqueue(element, _priorityFn(element));
    }
}