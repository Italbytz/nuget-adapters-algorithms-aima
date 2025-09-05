// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System;
using Italbytz.AI.Search.Framework;

namespace Italbytz.AI.Search.Adversarial;

/// <summary>
///     An algorithm for calculating minimax decisions with alpha-beta-pruning. It
///     returns the
///     action corresponding to the best possible move, that is, the move that
///     leads
///     to the outcome with the best utility, under the assumption that the
///     opponent
///     plays to minimize utility.
/// </summary>
/// <typeparam name="TState">Type which is used for states in the game.</typeparam>
/// <typeparam name="TAction">Type which is used for actions in the game.</typeparam>
/// <typeparam name="TPlayer">Type which is used for players in the game.</typeparam>
public class
    AlphaBetaSearch<TState, TAction, TPlayer> : IAdversarialSearch<TState,
    TAction>
{
    public const string MetricNodesExpanded = "nodesExpanded";

    private readonly IGame<TState, TAction, TPlayer> game;

    public AlphaBetaSearch(IGame<TState, TAction, TPlayer> game)
    {
        this.game = game;
    }

    public IMetrics Metrics { get; private set; } = new Metrics();

    public TAction MakeDecision(TState state)
    {
        Metrics = new Metrics();
        Metrics.Set(MetricNodesExpanded, 0);
        var result = default(TAction);
        var resultValue = double.NegativeInfinity;
        var player = game.Player(state);
        foreach (var action in game.Actions(state))
        {
            var value = MinValue(game.Result(state, action), player,
                double.NegativeInfinity, double.PositiveInfinity);
            if (!(value > resultValue)) continue;
            result = action;
            resultValue = value;
        }

        return result;
    }

    private double MinValue(TState state, TPlayer player, double alpha,
        double beta)
    {
        Metrics.IncrementInt(MetricNodesExpanded);
        if (game.Terminal(state)) return game.Utility(state, player);
        var value = double.PositiveInfinity;
        foreach (var action in game.Actions(state))
        {
            value = Math.Min(value,
                MaxValue(game.Result(state, action), player, alpha, beta));
            if (value <= alpha) return value;
            beta = Math.Min(beta, value);
        }

        return value;
    }

    private double MaxValue(TState state, TPlayer player, double alpha,
        double beta)
    {
        Metrics.IncrementInt(MetricNodesExpanded);
        if (game.Terminal(state)) return game.Utility(state, player);
        var value = double.NegativeInfinity;
        foreach (var action in game.Actions(state))
        {
            value = Math.Max(value,
                MinValue(game.Result(state, action), player, alpha, beta));
            if (value >= beta) return value;
            alpha = Math.Max(alpha, value);
        }

        return value;
    }


    public static MinimaxSearch<TState, TAction, TPlayer> CreateFor(
        IGame<TState, TAction, TPlayer> game)
    {
        return new MinimaxSearch<TState, TAction, TPlayer>(game);
    }
}