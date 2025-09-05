// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

namespace Italbytz.AI.Learning.Inductive;

public class ConstantDecisionTree : DecisionTree
{
    public ConstantDecisionTree(string value)
    {
        Value = value;
    }

    public string Value { get; set; }

    public override object Predict(IExample e)
    {
        return Value;
    }
}