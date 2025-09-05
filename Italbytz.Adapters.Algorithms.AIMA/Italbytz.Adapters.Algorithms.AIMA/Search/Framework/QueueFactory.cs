// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System;
using Italbytz.AI.Util.Datastructure;

namespace Italbytz.AI.Search.Framework;

public static class QueueFactory
{
    public static NodePriorityQueue<TState, TAction>
        CreatePriorityQueue<TState, TAction>(
            Func<INode<TState, TAction>, double> priorityFn)
    {
        return new(priorityFn, 11);
    }
}