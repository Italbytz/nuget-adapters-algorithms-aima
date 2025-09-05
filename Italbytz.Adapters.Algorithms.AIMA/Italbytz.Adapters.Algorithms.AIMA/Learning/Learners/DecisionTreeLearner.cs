// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System.Collections.Generic;
using System.Linq;
using Italbytz.AI.Learning.Inductive;

namespace Italbytz.AI.Learning.Learners;

public class DecisionTreeLearner : ILearner
{
    public DecisionTreeLearner()
    {
        DefaultValue = "Unable To Classify";
    }

    public DecisionTreeLearner(DecisionTree decisionTree, string defaultValue)
    {
        Tree = decisionTree;
        DefaultValue = defaultValue;
    }

    public DecisionTree Tree { get; set; }
    public string DefaultValue { get; set; }

    public void Train(IDataSet ds)
    {
        var attributes = ds.GetNonTargetAttributes();
        Tree = DecisionTreeLearning(ds, attributes,
            new ConstantDecisionTree(DefaultValue));
    }

    /*public string[] Predict(IDataSet ds)
    {
        var results = new string[ds.Examples.Count];
        for (var i = 0; i < ds.Examples.Count; i++)
            results[i] = Predict(ds.Examples[i]);
        return results;
    }*/

    public string Predict(IExample e)
    {
        return (string)Tree.Predict(e);
    }

    public int[] Test(IDataSet ds)
    {
        var results = new[] { 0, 0 };

        foreach (var e in ds.Examples)
            if (e.TargetValue().Equals(Tree.Predict(e)))
                results[0] = results[0] + 1;
            else
                results[1] = results[1] + 1;

        return results;
    }

    public string[] Predict(IDataSet ds)
    {
        return ds.Examples.Select(Predict).ToArray();
    }

    private DecisionTree DecisionTreeLearning(IDataSet ds,
        IEnumerable<string> attributeNames, ConstantDecisionTree defaultTree)
    {
        if (ds.Examples.Count == 0)
            return defaultTree;
        if (AllExamplesHaveSameClassification(ds))
            return new ConstantDecisionTree(ds.Examples[0].TargetValue());
        if (!attributeNames.Any())
            return MajorityValue(ds);
        var chosenAttribute = ChooseAttribute(ds, attributeNames);
        var tree = new DecisionTree(chosenAttribute);
        var m = MajorityValue(ds);
        var values = ds.GetPossibleAttributeValues(chosenAttribute);
        foreach (var v in values)
        {
            var filtered = ds.MatchingDataSet(chosenAttribute, v);

            var newAttribs =
                Util.Util.RemoveFrom(attributeNames, chosenAttribute);
            var subTree = DecisionTreeLearning(filtered, newAttribs, m);
            tree.AddNode(v, subTree);
        }

        return tree;
    }

    private static string ChooseAttribute(IDataSet ds,
        IEnumerable<string> attributeNames)
    {
        var greatestGain = 0.0;
        var attributeWithGreatestGain = attributeNames.First();
        foreach (var attr in attributeNames)
        {
            var gain = ds.CalculateGainFor(attr);
            if (!(gain > greatestGain)) continue;
            greatestGain = gain;
            attributeWithGreatestGain = attr;
        }

        return attributeWithGreatestGain;
    }

    private static ConstantDecisionTree MajorityValue(IDataSet ds)
    {
        var learner = new MajorityLearner();
        learner.Train(ds);
        return new ConstantDecisionTree(learner.Predict(ds.Examples[0]));
    }

    private static bool AllExamplesHaveSameClassification(IDataSet ds)
    {
        var classification = ds.Examples[0].TargetValue();
        return ds.Examples.All(ex => ex.TargetValue().Equals(classification));
    }
}