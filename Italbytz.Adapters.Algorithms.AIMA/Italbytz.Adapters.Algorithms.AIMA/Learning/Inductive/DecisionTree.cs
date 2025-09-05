// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System;
using System.Collections.Generic;
using System.Linq;

namespace Italbytz.AI.Learning.Inductive;

public class DecisionTree
{
    private readonly Dictionary<string, DecisionTree> _nodes;

    protected DecisionTree()
    {
    }

    public DecisionTree(string attributeName)
    {
        AttributeName = attributeName;
        if (attributeName == null)
            throw new ArgumentNullException(nameof(attributeName));
        _nodes = new Dictionary<string, DecisionTree>();
    }

    private string? AttributeName { get; }

    public void AddLeaf(string attributeValue, string decision)
    {
        _nodes[attributeValue] = new ConstantDecisionTree(decision);
    }

    public void AddNode(string attributeValue, DecisionTree tree)
    {
        _nodes[attributeValue] = tree;
    }

    public virtual object Predict(IExample e)
    {
        var attrValue = e.GetAttributeValueAsString(AttributeName);
        if (_nodes.ContainsKey(attrValue))
            return _nodes[attrValue].Predict(e);
        throw new Exception($"no node exists for attribute value {attrValue}");
    }

    public static DecisionTree GetStumpFor(IDataSet ds, string attributeName,
        string attributeValue, string returnValueIfMatched,
        List<string> unmatchedValues, string returnValueIfUnmatched)
    {
        var dt = new DecisionTree(attributeName);
        dt.AddLeaf(attributeValue, returnValueIfMatched);
        foreach (var unmatchedValue in unmatchedValues)
            dt.AddLeaf(unmatchedValue, returnValueIfUnmatched);
        return dt;
    }

    public static IEnumerable<DecisionTree> GetStumpsFor(IDataSet ds,
        string returnValueIfMatched, string returnValueIfUnmatched)
    {
        var attributes = ds.GetNonTargetAttributes();

        return (from attribute in attributes
            let values = ds.GetPossibleAttributeValues(attribute)
            from value in values
            let unmatchedValues =
                Util.Util.RemoveFrom(ds.GetPossibleAttributeValues(attribute),
                    value)
            select GetStumpFor(ds, attribute, value, returnValueIfMatched,
                unmatchedValues, returnValueIfUnmatched)).ToList();
    }
}