// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

namespace Italbytz.AI.Search.CSP;

public class Variable : IVariable
{
    public Variable(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public bool Equals(IVariable? other)
    {
        return other != null && Name.Equals(other.Name);
    }

    public override bool Equals(object? obj)
    {
        return obj is Variable variable && Equals(variable);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}