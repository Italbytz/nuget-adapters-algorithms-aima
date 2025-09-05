using Italbytz.AI.Logic.Planning;

namespace Italbytz.AI.Tests.Unit.Logic.Planning;

public class UtilsTest
{
    [Test]
    public void ParserTest()
    {
        var precondition = "At(C1,JFK) ^ At(C2,SFO)";
        var literals = Utils.Parse(precondition);
        Assert.That(literals.Count, Is.EqualTo(2));
        Assert.That(literals[0].ToString(), Is.EqualTo("At(C1,JFK)"));
        Assert.That(literals[1].ToString(), Is.EqualTo("At(C2,SFO)"));
    }
}