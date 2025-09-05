// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using Italbytz.AI.Learning.Learners;
using Italbytz.AI.Tests.Unit.Learning.Framework;

namespace Italbytz.AI.Tests.Unit.Learning.Learners;

public class LearnerTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestMajorityLearner()
    {
        var learner = new MajorityLearner();
        var ds = TestDataSetFactory.GetRestaurantDataSet();
        learner.Train(ds);
        var result = learner.Test(ds);
        Assert.Multiple(() =>
        {
            Assert.That(result[0], Is.EqualTo(6));
            Assert.That(result[1], Is.EqualTo(6));
        });
    }

    [Test]
    public void TestDefaultUsedWhenTrainingDataSetHasNoExamples()
    {
        // tests RecursionBaseCase#1
        var ds = TestDataSetFactory.GetRestaurantDataSet();
        var learner = new DecisionTreeLearner();

        var ds2 = ds.EmptyDataSet();
        Assert.That(ds2.Examples, Is.Empty);

        learner.Train(ds2);
        Assert.That(learner.Predict(ds.Examples[0]),
            Is.EqualTo("Unable To Classify"));
    }

    [Test]
    public void
        TestClassificationReturnedWhenAllExamplesHaveTheSameClassification()
    {
        // tests RecursionBaseCase#2
        var ds = TestDataSetFactory.GetRestaurantDataSet();
        var learner = new DecisionTreeLearner();

        var ds2 = ds.EmptyDataSet();

        // all 3 examples have the same classification (willWait = yes)
        ds2.Examples.Add(ds.Examples[0]);
        ds2.Examples.Add(ds.Examples[2]);
        ds2.Examples.Add(ds.Examples[3]);

        learner.Train(ds2);
        Assert.That(learner.Predict(ds.Examples[0]), Is.EqualTo("Yes"));
    }

    [Test]
    public void
        TestMajorityReturnedWhenAttributesToExamineIsEmpty()
    {
        // tests RecursionBaseCase#2
        var ds = TestDataSetFactory.GetRestaurantDataSet();
        var learner = new DecisionTreeLearner();

        var ds2 = ds.EmptyDataSet();

        // 3 examples have classification = "yes" and one ,"no"
        ds2.Examples.Add(ds.Examples[0]);
        ds2.Examples.Add(ds.Examples[1]); // "no"
        ds2.Examples.Add(ds.Examples[2]);
        ds2.Examples.Add(ds.Examples[3]);
        ds2.Specification = new MockDataSetSpecification("will_wait");

        learner.Train(ds2);
        Assert.That(learner.Predict(ds.Examples[1]), Is.EqualTo("Yes"));
    }

    [Test]
    public void
        TestInducedTreeClassifiesDataSetCorrectly()
    {
        var ds = TestDataSetFactory.GetRestaurantDataSet();
        var learner = new DecisionTreeLearner();
        learner.Train(ds);
        var result = learner.Test(ds);
        Assert.Multiple(() =>
        {
            Assert.That(result[0], Is.EqualTo(12));
            Assert.That(result[1], Is.EqualTo(0));
        });
    }
}