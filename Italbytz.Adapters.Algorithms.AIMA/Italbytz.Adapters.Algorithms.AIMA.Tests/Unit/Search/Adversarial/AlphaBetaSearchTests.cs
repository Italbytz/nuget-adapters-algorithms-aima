// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using Italbytz.AI.Search.Adversarial;
using Italbytz.AI.Tests.Environment.Map;
using Italbytz.AI.Tests.Environment.TwoPly;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Italbytz.AI.Tests.Unit.Search.Adversarial;

public class AlphaBetaSearchTests
{
    private const bool ConsoleLogging = false;

    private AlphaBetaSearch<TwoPlyGameState, MoveToAction, string>
        _alphaBetaSearch;

    private IGame<TwoPlyGameState, MoveToAction, string> _game;
    private ILoggerFactory _loggerFactory = NullLoggerFactory.Instance;

    [SetUp]
    public void Setup()
    {
        if (ConsoleLogging)
            _loggerFactory =
                LoggerFactory.Create(builder => builder.AddConsole());
        _game = new TwoPlyGame();
        _alphaBetaSearch =
            new AlphaBetaSearch<TwoPlyGameState, MoveToAction, string>(_game);
    }

    [TearDown]
    public void Cleanup()
    {
        _loggerFactory.Dispose();
    }

    [Test]
    public void TestUtilities()
    {
        var state = new TwoPlyGameState("E");
        Assert.That(_game.Utility(state, _game.Player(state)), Is.EqualTo(3));
        state = new TwoPlyGameState("I");
        Assert.That(_game.Utility(state, _game.Player(state)), Is.EqualTo(4));
        state = new TwoPlyGameState("K");
        Assert.That(_game.Utility(state, _game.Player(state)), Is.EqualTo(14));
    }

    [Test]
    public void TestMinimaxDecision()
    {
        Assert.Multiple(() =>
        {
            Assert.That(_game.InitialState.ToString(), Is.EqualTo("A"));
            Assert.That(_game.Player(_game.InitialState), Is.EqualTo("MAX"));
            Assert.That(
                _alphaBetaSearch.MakeDecision(_game.InitialState).ToString(),
                Is.EqualTo("MoveToAction[name=moveTo, location=B]"));
        });
    }
}