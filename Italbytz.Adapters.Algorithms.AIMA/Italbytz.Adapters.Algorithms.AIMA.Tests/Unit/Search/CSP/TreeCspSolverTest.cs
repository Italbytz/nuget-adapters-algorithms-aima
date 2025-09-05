using Italbytz.AI.Search.CSP;
using Italbytz.AI.Search.CSP.Examples;
using Italbytz.AI.Search.CSP.Solver;

namespace Italbytz.AI.Tests.Unit.Search.CSP;

public class TreeCspSolverTest
{
    private static readonly IVariable WA = new Variable("WA");
    private static readonly IVariable NT = new Variable("NT");
    private static readonly IVariable Q = new Variable("Q");
    private static readonly IVariable NSW = new Variable("NSW");
    private static readonly IVariable V = new Variable("V");

    private static readonly IConstraint<IVariable, string> C1 =
        new NotEqualConstraint<IVariable, string>(WA, NT);

    private static readonly IConstraint<IVariable, string> C2 =
        new NotEqualConstraint<IVariable, string>(NT, Q);

    private static readonly IConstraint<IVariable, string> C3 =
        new NotEqualConstraint<IVariable, string>(Q, NSW);

    private static readonly IConstraint<IVariable, string> C4 =
        new NotEqualConstraint<IVariable, string>(NSW, V);

    private IDomain<string> _colors;
    private IList<IVariable> _variables;

    [SetUp]
    public void SetUp()
    {
        _variables = new List<IVariable>
        {
            WA,
            NT,
            Q,
            NSW,
            V
        };

        _colors = new Domain<string>("red", "green", "blue");
    }

    [Test]
    public void TestConstraintNetwork()
    {
        var csp = new CSP<IVariable, string>(_variables);
        csp.AddConstraint(C1);
        csp.AddConstraint(C2);
        csp.AddConstraint(C3);
        csp.AddConstraint(C4);
        Assert.That(csp.Constraints, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(csp.Constraints, Has.Count.EqualTo(4));
            Assert.That(csp.GetConstraints(WA), Is.Not.Null);
            Assert.That(csp.GetConstraints(WA), Has.Count.EqualTo(1));
            Assert.That(csp.GetConstraints(NT), Is.Not.Null);
            Assert.That(csp.GetConstraints(NT), Has.Count.EqualTo(2));
            Assert.That(csp.GetConstraints(Q), Is.Not.Null);
            Assert.That(csp.GetConstraints(Q), Has.Count.EqualTo(2));
            Assert.That(csp.GetConstraints(NSW), Is.Not.Null);
            Assert.That(csp.GetConstraints(NSW), Has.Count.EqualTo(2));
        });
    }

    [Test]
    public void TestDomainChanges()
    {
        var colors2 = new Domain<string>("red", "green", "blue");
        Assert.That(colors2, Is.EqualTo(_colors));
        var csp = new CSP<IVariable, string>(_variables);
        csp.AddConstraint(C1);
        Assert.That(csp.GetDomain(WA), Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(csp.GetDomain(WA), Is.Empty);
            Assert.That(csp.GetConstraints(WA), Is.Not.Null);
        });

        csp.SetDomain(WA, _colors);
        Assert.That(csp.GetDomain(WA), Is.EqualTo(_colors));
        Assert.That(csp.GetDomain(WA), Has.Count.EqualTo(3));
        Assert.That(csp.GetDomain(WA), Has.Member("red"));

        var cspCopy = csp.CopyDomains();
        Assert.That(cspCopy.GetDomain(WA), Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(cspCopy.GetDomain(WA), Has.Count.EqualTo(3));
            Assert.That(cspCopy.GetDomain(WA), Has.Member("red"));
            Assert.That(cspCopy.GetDomain(NT), Is.Not.Null);
            Assert.That(cspCopy.GetDomain(NT), Is.Empty);
            Assert.That(cspCopy.GetConstraints(NT), Is.Not.Null);
            Assert.That(cspCopy.GetConstraints(NT).First(), Is.EqualTo(C1));
        });

        cspCopy.RemoveValueFromDomain(WA, "red");
        Assert.Multiple(() =>
        {
            Assert.That(cspCopy.GetDomain(WA), Has.Count.EqualTo(2));
            Assert.That(cspCopy.GetDomain(WA), Has.Member("green"));
            Assert.That(csp.GetDomain(WA), Has.Count.EqualTo(3));
            Assert.That(csp.GetDomain(WA), Has.Member("red"));
        });
    }

    [Test]
    public void TestTreeCspSolver()
    {
        var csp = new CSP<IVariable, string>(_variables);
        csp.AddConstraint(C1);
        csp.AddConstraint(C2);
        csp.AddConstraint(C3);
        csp.AddConstraint(C4);

        csp.SetDomain(WA, _colors);
        csp.SetDomain(NT, _colors);
        csp.SetDomain(Q, _colors);
        csp.SetDomain(NSW, _colors);
        csp.SetDomain(V, _colors);

        var solver = new TreeCspSolver<IVariable, string>();
        var assignment = solver.Solve(csp);
        Assert.That(assignment, Is.Not.Null);
        Assert.That(assignment.IsComplete(csp.Variables), Is.True);
        Assert.That(assignment.IsSolution(csp), Is.True);
    }
}