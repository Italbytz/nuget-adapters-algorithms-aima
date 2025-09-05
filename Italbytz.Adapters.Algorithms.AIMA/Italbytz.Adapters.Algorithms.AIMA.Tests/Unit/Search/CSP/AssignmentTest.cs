// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using Italbytz.AI.Search.CSP;

namespace Italbytz.AI.Tests.Unit.Search.CSP;

public class AssignmentTest
{
    private static readonly IVariable X = new Variable("x");
    private static readonly IVariable Y = new Variable("y");
    private Assignment<IVariable, string> _assignment;

    private List<IVariable> _variables;

    [SetUp]
    public void SetUp()
    {
        _variables = [X, Y];
        _assignment = new Assignment<IVariable, string>();
    }

    [Test]
    public void TestGetValue()
    {
        _assignment.Add(X, "red");
        Assert.That(_assignment.GetValue(X), Is.EqualTo("red"));
    }

    [Test]
    public void TestRemove()
    {
        _assignment.Add(X, "red");
        _assignment.Remove(X);
    }

    [Test]
    public void TestIsComplete()
    {
        _assignment.Add(X, "red");
        Assert.That(_assignment.IsComplete(_variables), Is.False);
        _assignment.Add(Y, "blue");
        Assert.That(_assignment.IsComplete(_variables), Is.True);
        _assignment.Remove(Y);
        Assert.That(_assignment.IsComplete(_variables), Is.False);
    }
}