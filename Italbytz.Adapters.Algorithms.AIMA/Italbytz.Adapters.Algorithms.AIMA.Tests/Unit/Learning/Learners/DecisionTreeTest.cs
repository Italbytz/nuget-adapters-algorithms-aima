// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System.Text;
using Italbytz.AI.Learning.Inductive;
using Italbytz.AI.Learning.Learners;
using Italbytz.AI.Tests.Unit.Learning.Framework;

namespace Italbytz.AI.Tests.Unit.Learning.Learners;

public class DecisionTreeTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestActualDecisionTreeClassifiesRestaurantDataSetCorrectly()
    {
        var learner = new DecisionTreeLearner(
            CreateActualRestaurantDecisionTree(), "Unable to classify");
        var results = learner.Test(TestDataSetFactory.GetRestaurantDataSet());
        Assert.Multiple(() =>
        {
            Assert.That(results[0], Is.EqualTo(12));
            Assert.That(results[1], Is.EqualTo(0));
        });
    }

    [Test]
    public void SavePredictionsInFile()
    {
        var completeRestaurantDataSet =
            TestDataSetFactory.GetCompleteRestaurantDataSet();
        var actualLearner = new DecisionTreeLearner(
            CreateActualRestaurantDecisionTree(), "Unable to classify");
        var actualPredictions =
            actualLearner.Predict(completeRestaurantDataSet);

        var sb = new StringBuilder();
        var attributeNames =
            completeRestaurantDataSet.Specification.GetAttributeNames()
                .ToArray();
        sb.Append(string.Join(", ", attributeNames));
        sb.Append("\n");
        var exampleNo = 0;
        foreach (var example in completeRestaurantDataSet.Examples)
        {
            foreach (var attribute in attributeNames)
                if (attribute.Equals("will_wait"))
                {
                    sb.Append(actualPredictions[exampleNo]);
                }
                else
                {
                    sb.Append(example.GetAttributeValueAsString(attribute));
                    sb.Append(", ");
                }

            sb.Append("\n");
            exampleNo++;
        }

        using var file = new StreamWriter(@"restaurantcomplete.csv");
        file.WriteLine(sb.ToString());
    }

    [Test]
    public void TestMisclassificationOfInducedDecisionTree()
    {
        var completeRestaurantDataSet =
            TestDataSetFactory.GetCompleteRestaurantDataSet();
        var actualLearner = new DecisionTreeLearner(
            CreateActualRestaurantDecisionTree(), "Unable to classify");
        var inducedLearner = new DecisionTreeLearner(
            CreateInducedRestaurantDecisionTree(), "Unable to classify");
        var actualPredictions =
            actualLearner.Predict(completeRestaurantDataSet);
        var inducedPredictions =
            inducedLearner.Predict(completeRestaurantDataSet);
        var mcr = 0F;
        for (var i = 0; i < actualPredictions.Length; i++)
            if (!actualPredictions[i].Equals(inducedPredictions[i]))
                mcr++;
        mcr /= actualPredictions.Length;
        Assert.That(mcr, Is.InRange(0.18, 0.19));
    }

    [Test]
    public void TestInducedDecisionTreeClassifiesRestaurantDataSetCorrectly()
    {
        var learner = new DecisionTreeLearner(
            CreateInducedRestaurantDecisionTree(), "Unable to classify");
        var results = learner.Test(TestDataSetFactory.GetRestaurantDataSet());
        Assert.Multiple(() =>
        {
            Assert.That(results[0], Is.EqualTo(12));
            Assert.That(results[1], Is.EqualTo(0));
        });
    }

    [Test]
    public void TestStumpCreationForSpecifiedAttributeValuePair()
    {
        var ds = TestDataSetFactory.GetRestaurantDataSet();
        var unmatchedValues = new List<string>();
        unmatchedValues.Add(AI.Util.Util.No);
        var dt = DecisionTree.GetStumpFor(ds, "alternate", AI.Util.Util.Yes,
            AI.Util.Util.Yes,
            unmatchedValues, AI.Util.Util.No);
        Assert.That(dt, Is.Not.Null);
    }

    [Test]
    public void TestStumpCreationForDataSet()
    {
        var ds = TestDataSetFactory.GetRestaurantDataSet();
        var dt = DecisionTree.GetStumpsFor(ds, AI.Util.Util.Yes,
            "Unable to classify");
        Assert.That(dt.Count, Is.EqualTo(26));
    }

    [Test]
    public void TestStumpPredictionForDataSet()
    {
        var ds = TestDataSetFactory.GetRestaurantDataSet();
        var unmatchedValues = new List<string> { AI.Util.Util.No };
        var tree = DecisionTree.GetStumpFor(ds, "hungry", AI.Util.Util.Yes,
            AI.Util.Util.Yes,
            unmatchedValues, "Unable to Classify");
        var learner = new DecisionTreeLearner(tree, "Unable to Classify");
        var result = learner.Test(ds);
        Assert.Multiple(() =>
        {
            Assert.That(result[0], Is.EqualTo(5));
            Assert.That(result[1], Is.EqualTo(7));
        });
    }

    private static DecisionTree CreateInducedRestaurantDecisionTree()
    {
        var frisat = new DecisionTree("fri/sat");
        frisat.AddLeaf(AI.Util.Util.Yes, AI.Util.Util.Yes);
        frisat.AddLeaf(AI.Util.Util.No, AI.Util.Util.No);

        // type node
        var type = new DecisionTree("type");
        type.AddLeaf("French", AI.Util.Util.Yes);
        type.AddLeaf("Italian", AI.Util.Util.No);
        type.AddNode("Thai", frisat);
        type.AddLeaf("Burger", AI.Util.Util.Yes);

        // hungry node
        var hungry = new DecisionTree("hungry");
        hungry.AddLeaf(AI.Util.Util.No, AI.Util.Util.No);
        hungry.AddNode(AI.Util.Util.Yes, type);

        // patrons node
        var patrons = new DecisionTree("patrons");
        patrons.AddLeaf("None", AI.Util.Util.No);
        patrons.AddLeaf("Some", AI.Util.Util.Yes);
        patrons.AddNode("Full", hungry);

        return patrons;
    }

    private static DecisionTree CreateActualRestaurantDecisionTree()
    {
        // raining node
        var raining = new DecisionTree("raining");
        raining.AddLeaf(AI.Util.Util.Yes, AI.Util.Util.Yes);
        raining.AddLeaf(AI.Util.Util.No, AI.Util.Util.No);

        // bar node
        var bar = new DecisionTree("bar");
        bar.AddLeaf(AI.Util.Util.Yes, AI.Util.Util.Yes);
        bar.AddLeaf(AI.Util.Util.No, AI.Util.Util.No);

        // friday saturday node
        var frisat = new DecisionTree("fri/sat");
        frisat.AddLeaf(AI.Util.Util.Yes, AI.Util.Util.Yes);
        frisat.AddLeaf(AI.Util.Util.No, AI.Util.Util.No);

        // second alternate node to the right of the diagram below hungry
        var alternate2 = new DecisionTree("alternate");
        alternate2.AddNode(AI.Util.Util.Yes, raining);
        alternate2.AddLeaf(AI.Util.Util.No, AI.Util.Util.Yes);

        // reservation node
        var reservation = new DecisionTree("reservation");
        reservation.AddNode(AI.Util.Util.No, bar);
        reservation.AddLeaf(AI.Util.Util.Yes, AI.Util.Util.Yes);

        // first alternate node to the left of the diagram below waitestimate
        var alternate1 = new DecisionTree("alternate");
        alternate1.AddNode(AI.Util.Util.No, reservation);
        alternate1.AddNode(AI.Util.Util.Yes, frisat);

        // hungry node
        var hungry = new DecisionTree("hungry");
        hungry.AddLeaf(AI.Util.Util.No, AI.Util.Util.Yes);
        hungry.AddNode(AI.Util.Util.Yes, alternate2);

        // wait estimate node
        var waitEstimate = new DecisionTree("wait_estimate");
        waitEstimate.AddLeaf(">60", AI.Util.Util.No);
        waitEstimate.AddNode("30-60", alternate1);
        waitEstimate.AddNode("10-30", hungry);
        waitEstimate.AddLeaf("0-10", AI.Util.Util.Yes);

        // patrons node
        var patrons = new DecisionTree("patrons");
        patrons.AddLeaf("None", AI.Util.Util.No);
        patrons.AddLeaf("Some", AI.Util.Util.Yes);
        patrons.AddNode("Full", waitEstimate);

        return patrons;
    }
}