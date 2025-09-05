// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System.Diagnostics;

namespace Italbytz.AI.Agent;

/// <inheritdoc cref="IEnvironment{TPercept,TAction}" />
public abstract class
    AbstractEnvironment<TPercept, TAction> : IEnvironment<TPercept, TAction>
{
    public IAgent<TPercept, TAction>? Agent { get; set; }

    public void Step()
    {
        Debug.Assert(Agent != null, nameof(Agent) + " != null");
        if (!Agent.Alive) return;
        var percept = GetPerceptSeenBy(Agent);
        var anAction = Agent.Act(percept);
        if (anAction != null) Execute(Agent, anAction);
    }

    protected abstract void Execute(IAgent<TPercept, TAction> agent,
        TAction? anAction);

    protected abstract TPercept? GetPerceptSeenBy(
        IAgent<TPercept, TAction> agent);
}