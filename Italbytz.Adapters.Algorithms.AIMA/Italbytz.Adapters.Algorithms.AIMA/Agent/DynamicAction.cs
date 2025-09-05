// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

namespace Italbytz.AI.Agent;

/// <inheritdoc cref="IAction" />
public class DynamicAction : ObjectWithDynamicAttributes, IAction
{
    private const string AttributeName = "name";


    protected DynamicAction(string name)
    {
        Attributes[AttributeName] = name;
    }

    public string Name => (string)Attributes[AttributeName];
}