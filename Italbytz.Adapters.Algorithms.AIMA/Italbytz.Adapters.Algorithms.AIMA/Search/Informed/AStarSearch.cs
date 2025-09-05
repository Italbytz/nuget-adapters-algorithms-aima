// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System;
using Italbytz.AI.Search.Framework.QSearch;

namespace Italbytz.AI.Search.Informed;

public class AStarSearch<TState, TAction> : BestFirstSearch<TState, TAction>
{
    public AStarSearch(QueueSearch<TState, TAction> impl,
        Func<INode<TState, TAction>, double> heuristicFn) : base(impl,
        CreateEvalFn(heuristicFn))
    {
    }

    private static Func<INode<TState, TAction>, double> CreateEvalFn(
        Func<INode<TState, TAction>, double> heuristicFn)
    {
        return node => node.PathCost + heuristicFn(node);
    }
}