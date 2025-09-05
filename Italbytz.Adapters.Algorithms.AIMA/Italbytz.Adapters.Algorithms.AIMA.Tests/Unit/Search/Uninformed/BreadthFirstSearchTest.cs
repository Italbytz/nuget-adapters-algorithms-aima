using Italbytz.Adapters.Algorithms.AI.Search.Uninformed;
using Italbytz.AI.Agent;
using Italbytz.AI.Search.Agent;
using Italbytz.AI.Search.Framework.Problem;
using Italbytz.AI.Tests.Environment.NQueens;
using Italbytz.AI.Util.Datastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Italbytz.Adapters.Algorithms.AI.Tests.Unit.Search.Uninformed;

public class BreadthFirstSearchTest
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
    public void TestQueensBoardDefaultSuccess()
    {
        var board = new NQueensBoard(8);
        board.AddQueenAt(new XYLocation(0, 0));
        board.AddQueenAt(new XYLocation(1, 4));
        board.AddQueenAt(new XYLocation(2, 7));
        board.AddQueenAt(new XYLocation(3, 5));
        board.AddQueenAt(new XYLocation(4, 2));
        board.AddQueenAt(new XYLocation(5, 6));
        board.AddQueenAt(new XYLocation(6, 1));
        board.AddQueenAt(new XYLocation(7, 3));
        var agent = TestNQueens(board);
        var env = new NQueensEnvironment(board) { Agent = agent };
        while (!agent.Done) env.Step();
        // Optimal solution
        Assert.That(env.Board.GetNumberOfAttackingPairs(), Is.EqualTo(0));
    }

    [Test]
    public void TestQueensBoardDepth1Success()
    {
        var board = new NQueensBoard(8);
        board.AddQueenAt(new XYLocation(0, 0));
        board.AddQueenAt(new XYLocation(1, 3));
        board.AddQueenAt(new XYLocation(2, 7));
        board.AddQueenAt(new XYLocation(3, 5));
        board.AddQueenAt(new XYLocation(4, 2));
        board.AddQueenAt(new XYLocation(5, 6));
        board.AddQueenAt(new XYLocation(6, 1));
        board.AddQueenAt(new XYLocation(7, 3));
        var agent = TestNQueens(board);
        var env = new NQueensEnvironment(board) { Agent = agent };
        while (!agent.Done) env.Step();
        Assert.That(env.Board.GetNumberOfAttackingPairs(), Is.EqualTo(0));
    }

    [Test]
    public void TestQueensBoardDepth2Success()
    {
        var board = new NQueensBoard(8);
        board.AddQueenAt(new XYLocation(0, 0));
        board.AddQueenAt(new XYLocation(1, 3));
        board.AddQueenAt(new XYLocation(2, 6));
        board.AddQueenAt(new XYLocation(3, 5));
        board.AddQueenAt(new XYLocation(4, 2));
        board.AddQueenAt(new XYLocation(5, 6));
        board.AddQueenAt(new XYLocation(6, 1));
        board.AddQueenAt(new XYLocation(7, 3));
        var agent = TestNQueens(board);
        var env = new NQueensEnvironment(board) { Agent = agent };
        while (!agent.Done) env.Step();
        Assert.That(env.Board.GetNumberOfAttackingPairs(), Is.EqualTo(0));
    }

    [Test]
    public void TestQueensBoardDepth3Success()
    {
        var board = new NQueensBoard(8);
        board.AddQueenAt(new XYLocation(0, 0));
        board.AddQueenAt(new XYLocation(1, 3));
        board.AddQueenAt(new XYLocation(2, 6));
        board.AddQueenAt(new XYLocation(3, 1));
        board.AddQueenAt(new XYLocation(4, 2));
        board.AddQueenAt(new XYLocation(5, 6));
        board.AddQueenAt(new XYLocation(6, 1));
        board.AddQueenAt(new XYLocation(7, 3));
        var agent = TestNQueens(board);
        var env = new NQueensEnvironment(board) { Agent = agent };
        while (!agent.Done) env.Step();
        Assert.That(env.Board.GetNumberOfAttackingPairs(), Is.EqualTo(0));
    }

    [Test]
    public void TestQueensBoardDepth4Success()
    {
        var board = new NQueensBoard(8);
        board.AddQueenAt(new XYLocation(0, 0));
        board.AddQueenAt(new XYLocation(1, 3));
        board.AddQueenAt(new XYLocation(2, 6));
        board.AddQueenAt(new XYLocation(3, 1));
        board.AddQueenAt(new XYLocation(4, 4));
        board.AddQueenAt(new XYLocation(5, 6));
        board.AddQueenAt(new XYLocation(6, 1));
        board.AddQueenAt(new XYLocation(7, 3));
        var agent = TestNQueens(board);
        var env = new NQueensEnvironment(board) { Agent = agent };
        while (!agent.Done) env.Step();
        Assert.That(env.Board.GetNumberOfAttackingPairs(), Is.EqualTo(0));
    }

    private SearchAgent<IPercept, NQueensBoard, QueenAction> TestNQueens(
        NQueensBoard board)
    {
        var problem = new GeneralProblem<NQueensBoard, QueenAction>(board,
            NQueensFunctions.GetCSFActions, NQueensFunctions.GetResult,
            NQueensFunctions.TestGoal);
        var search = new BreadthFirstSearch<NQueensBoard, QueenAction>();
        var agent = new SearchAgent<IPercept, NQueensBoard, QueenAction>
            (problem, search, _loggerFactory);
        return agent;
    }
}