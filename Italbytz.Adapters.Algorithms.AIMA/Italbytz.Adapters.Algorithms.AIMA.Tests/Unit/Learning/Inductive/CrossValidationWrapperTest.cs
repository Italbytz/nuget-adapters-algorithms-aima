using Italbytz.AI.Learning.Inductive;
using Italbytz.AI.Tests.Mock.Learning.Learners;
using Italbytz.AI.Tests.Unit.Learning.Framework;

namespace Italbytz.AI.Tests.Unit.Learning.Inductive;

public class CrossValidationWrapperTest
{
    [Test]
    public void CrossValidationWrapperTestCase()
    {
        var validation = new CrossValidation(0.05);
        // This learner gives least error when size param is 70
        var result = validation.CrossValidationWrapper(
            new SampleParameterizedLearner(), 5,
            TestDataSetFactory.GetRestaurantDataSet());
        Assert.That(result.ParameterSize, Is.EqualTo(70));
    }
}