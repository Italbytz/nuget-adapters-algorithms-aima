using Italbytz.AI.Search.CSP;
using Italbytz.AI.Search.CSP.Examples;

namespace Italbytz.AI.Tests.Unit.Search.CSP;

public class CSPTest
{
    private static readonly IVariable X = new Variable("x");
    private static readonly IVariable Y = new Variable("y");
    private static readonly IVariable Z = new Variable("z");

    private static readonly IConstraint<IVariable, string> C1 =
        new NotEqualConstraint<IVariable, string>(X, Y);

    private static readonly IConstraint<IVariable, string> C2 =
        new NotEqualConstraint<IVariable, string>(X, Y);

    private IDomain<string> _animals;

    private IDomain<string> _colors;

    private IList<IVariable> _variables;


    [SetUp]
    public void SetUp()
    {
        _variables =
        [
            X,
            Y,
            Z
        ];

        _colors = new Domain<string>("red", "green", "blue");
        _animals = new Domain<string>("cat", "dog");
    }

    [Test]
    public void TestConstraintNetwork()
    {
        var csp = new CSP<IVariable, string>(_variables);
        csp.AddConstraint(C1);
        csp.AddConstraint(C2);

        Assert.Multiple(() =>
        {
            Assert.That(csp.Constraints, Is.Not.Null);
            Assert.That(csp.Constraints, Has.Count.EqualTo(2));
            Assert.That(csp.GetConstraints(X), Is.Not.Null);
            Assert.That(csp.GetConstraints(X), Has.Count.EqualTo(2));
            Assert.That(csp.GetConstraints(Y), Is.Not.Null);
            Assert.That(csp.GetConstraints(Y), Has.Count.EqualTo(2));
            Assert.That(csp.GetConstraints(Z), Is.Not.Null);
            Assert.That(csp.GetConstraints(Z), Is.Empty);
        });
    }

    [Test]
    public void TestGetNeighbor()
    {
        var csp = new CSP<IVariable, string>(_variables);
        csp.AddConstraint(C1);
        csp.AddConstraint(C2);
        Assert.Multiple(() =>
        {
            Assert.That(csp.GetNeighbor(X, C1), Is.EqualTo(Y));
            Assert.That(csp.GetNeighbor(Y, C1), Is.EqualTo(X));
        });
    }

    [Test]
    public void TestRemoveConstraint()
    {
        var csp = new CSP<IVariable, string>(_variables);
        csp.AddConstraint(C1);
        csp.AddConstraint(C2);
        Assert.Multiple(() =>
        {
            Assert.That(csp.Constraints, Has.Count.EqualTo(2));
            Assert.That(csp.RemoveConstraint(C1), Is.True);
            Assert.That(csp.Constraints, Has.Count.EqualTo(1));
            Assert.That(csp.RemoveConstraint(C1), Is.False);
            Assert.That(csp.Constraints, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void TestDomainChanges()
    {
        var colors2 = new Domain<string>(_colors.Values);
        Assert.That(colors2, Is.EqualTo(_colors));

        var csp = new CSP<IVariable, string>(_variables);
        csp.AddConstraint(C1);
        Assert.Multiple(() =>
        {
            Assert.That(csp.GetDomain(X), Is.Not.Null);
            Assert.That(csp.GetDomain(X), Has.Count.EqualTo(0));
            Assert.That(csp.GetConstraints(X), Is.Not.Null);
        });

        csp.SetDomain(X, _colors);
        Assert.Multiple(() =>
        {
            Assert.That(csp.GetDomain(X), Is.EqualTo(_colors));
            Assert.That(csp.GetDomain(X), Has.Count.EqualTo(3));
            Assert.That(csp.GetDomain(X)[0], Is.EqualTo("red"));
        });

        var cspCopy = csp.CopyDomains();
        Assert.That(cspCopy.GetDomain(X), Is.Not.Null);
        var xConst = cspCopy.GetConstraints(X);
        Assert.Multiple(() =>
        {
            Assert.That(cspCopy.GetDomain(X), Has.Count.EqualTo(3));
            Assert.That(cspCopy.GetDomain(X)[0], Is.EqualTo("red"));
            Assert.That(cspCopy.GetDomain(Y), Is.Not.Null);
            Assert.That(cspCopy.GetDomain(Y), Has.Count.EqualTo(0));
            Assert.That(cspCopy.GetConstraints(X), Is.Not.Null);
            Assert.That(cspCopy.GetConstraints(X)[0], Is.EqualTo(C1));
        });

        cspCopy.RemoveValueFromDomain(X, "red");
        Assert.Multiple(() =>
        {
            Assert.That(cspCopy.GetDomain(X), Has.Count.EqualTo(2));
            Assert.That(cspCopy.GetDomain(X)[0], Is.EqualTo("green"));
            Assert.That(csp.GetDomain(X), Has.Count.EqualTo(3));
            Assert.That(csp.GetDomain(X)[0], Is.EqualTo("red"));
        });

        cspCopy.SetDomain(X, _animals);
        Assert.Multiple(() =>
        {
            Assert.That(cspCopy.GetDomain(X), Has.Count.EqualTo(2));
            Assert.That(cspCopy.GetDomain(X)[0], Is.EqualTo("cat"));
            Assert.That(csp.GetDomain(X), Has.Count.EqualTo(3));
            Assert.That(csp.GetDomain(X)[0], Is.EqualTo("red"));
        });
    }
}