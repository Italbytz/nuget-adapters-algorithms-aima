// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System.Collections.Generic;
using Italbytz.AI.Agent;
using Italbytz.AI.Problem;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Italbytz.AI.Search.Agent;

public class
    SearchAgent<TPercept, TState, TAction> : SimpleAgent<TPercept, TAction>
{
    private readonly Queue<TAction> actionsQueue = new();
    private IMetrics _searchMetrics;

    public SearchAgent(IProblem<TState, TAction> problem,
        ISearchForActions<TState, TAction> search) : this(problem, search,
        NullLoggerFactory.Instance)
    {
    }

    public SearchAgent(IProblem<TState, TAction> problem,
        ISearchForActions<TState, TAction> search,
        ILoggerFactory loggerFactory) : base(loggerFactory)
    {
        var actions = search.FindActions(problem);
        Actions = new List<TAction>();
        if (actions != null) Actions.AddRange(actions);
        actionsQueue = new Queue<TAction>(Actions);
        _searchMetrics = search.Metrics;
    }

    public List<TAction> Actions { get; }

    //private List<TAction>.Enumerator actionEnumerator;
    public bool Done => actionsQueue.Count == 0;

    public override TAction? Act(TPercept? percept)
    {
        return actionsQueue.Count > 0 ? actionsQueue.Dequeue() : default;
    }
}