using Italbytz.AI.Agent;
using Italbytz.AI.Search.Agent;
using Italbytz.AI.Search.Framework.Problem;
using Italbytz.AI.Search.Local;
using Italbytz.AI.Tests.Environment.NQueens;
using Italbytz.AI.Util.Datastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Italbytz.AI.Tests.Unit.Search.Local;

public class NQueensSimulatedAnnealingTests
{
    private const bool ConsoleLogging = true;
    private ILoggerFactory _loggerFactory = NullLoggerFactory.Instance;

    [SetUp]
    public void Setup()
    {
        if (ConsoleLogging)
            _loggerFactory =
                LoggerFactory.Create(builder => builder.AddConsole());
    }

    [TearDown]
    public void Cleanup()
    {
        _loggerFactory.Dispose();
    }

    [Test]
    public void TestNQueensBoard1()
    {
        var board = new NQueensBoard(8);
        for (var i = 0; i < board.Size; i++)
            board.AddQueenAt(new XYLocation(i, 0));
        var agent = TestNQueens(board);
        var env = new NQueensEnvironment(board) { Agent = agent };
        while (!agent.Done) env.Step();
        // Optimal solution is not guaranteed
        Assert.That(env.Board.GetNumberOfAttackingPairs(), Is.AnyOf(0, 28));
    }

    [Test]
    public void TestNQueensBoard2()
    {
        var board = new NQueensBoard(8);
        board.AddQueenAt(new XYLocation(0, 4));
        board.AddQueenAt(new XYLocation(1, 5));
        board.AddQueenAt(new XYLocation(2, 6));
        board.AddQueenAt(new XYLocation(3, 3));
        board.AddQueenAt(new XYLocation(4, 4));
        board.AddQueenAt(new XYLocation(5, 5));
        board.AddQueenAt(new XYLocation(6, 6));
        board.AddQueenAt(new XYLocation(7, 5));
        var agent = TestNQueens(board);
        var env = new NQueensEnvironment(board) { Agent = agent };
        while (!agent.Done) env.Step();
        // Optimal solution is not guaranteed
        Assert.That(env.Board.GetNumberOfAttackingPairs(), Is.AnyOf(0, 17));
    }

    [Test]
    public void TestNQueensBoard3()
    {
        var board = new NQueensBoard(8);
        board.AddQueenAt(new XYLocation(0, 5));
        board.AddQueenAt(new XYLocation(1, 7));
        board.AddQueenAt(new XYLocation(2, 0));
        board.AddQueenAt(new XYLocation(3, 1));
        board.AddQueenAt(new XYLocation(4, 1));
        board.AddQueenAt(new XYLocation(5, 7));
        board.AddQueenAt(new XYLocation(6, 7));
        board.AddQueenAt(new XYLocation(7, 2));
        var agent = TestNQueens(board);
        var env = new NQueensEnvironment(board) { Agent = agent };
        while (!agent.Done) env.Step();
        // Optimal solution is not guaranteed
        Assert.That(env.Board.GetNumberOfAttackingPairs(), Is.AnyOf(0, 6));
    }

    private SearchAgent<IPercept, NQueensBoard, QueenAction> TestNQueens(
        NQueensBoard board)
    {
        var problem = new GeneralProblem<NQueensBoard, QueenAction>(board,
            NQueensFunctions.GetCSFActions, NQueensFunctions.GetResult,
            NQueensFunctions.TestGoal);
        var search = new SimulatedAnnealingSearch<NQueensBoard, QueenAction>(
            NQueensFunctions.GetNumberOfAttackingPairs,
            new Scheduler(20, 0.045, 1000));
        var agent = new SearchAgent<IPercept, NQueensBoard, QueenAction>(
            problem, search, _loggerFactory);
        return agent;
    }
}