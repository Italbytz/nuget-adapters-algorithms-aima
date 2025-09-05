using Italbytz.AI.Search.CSP;
using Italbytz.AI.Search.CSP.Examples;
using Italbytz.AI.Search.CSP.Solver;

namespace Italbytz.AI.Tests.Unit.Search.CSP;

public class MapCSPTest
{
    private ICSP<Variable, string> csp;

    [SetUp]
    public void SetUp()
    {
        csp = new MapCSP();
    }

    [Test]
    public void TestBackTrackingSearch()
    {
        var solver = new FlexibleBacktrackingSolver<Variable, string>();
        var assignment = solver.Solve(csp);
        Assert.That(assignment, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(assignment.GetValue(MapCSP.WA),
                Is.EqualTo(MapCSP.BLUE));
            Assert.That(assignment.GetValue(MapCSP.NT), Is.EqualTo(MapCSP.RED));
            Assert.That(assignment.GetValue(MapCSP.SA),
                Is.EqualTo(MapCSP.GREEN));
            Assert.That(assignment.GetValue(MapCSP.Q), Is.EqualTo(MapCSP.BLUE));
            Assert.That(assignment.GetValue(MapCSP.NSW),
                Is.EqualTo(MapCSP.RED));
            Assert.That(assignment.GetValue(MapCSP.V), Is.EqualTo(MapCSP.BLUE));
            Assert.That(assignment.GetValue(MapCSP.T), Is.EqualTo(MapCSP.RED));
        });
    }

    [Test]
    public void TestMCSearch()
    {
        var assignment =
            new MinConflictsSolver<Variable, string>(100).Solve(csp);
        Assert.That(assignment, Is.Not.Null);
    }
}