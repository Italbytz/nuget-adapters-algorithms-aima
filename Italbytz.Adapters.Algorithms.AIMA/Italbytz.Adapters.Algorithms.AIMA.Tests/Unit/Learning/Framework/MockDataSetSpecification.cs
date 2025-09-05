using Italbytz.AI.Learning.Framework;

namespace Italbytz.AI.Tests.Unit.Learning.Framework;

public class MockDataSetSpecification : DataSetSpecification
{
    public MockDataSetSpecification(string targetAttributeName)
    {
        TargetAttribute = targetAttributeName;
    }
}