using System;
using System.Collections.Generic;
using System.Linq;
using Italbytz.AI.Logic.Propositional.KB.Data;
using Italbytz.AI.Logic.Propositional.Parsing.Ast;

namespace Italbytz.AI.Logic.Propositional.Inference;

/// <summary>
///     Implementation of the WalkSAT algorithm for propositional satisfiability.
/// </summary>
public class WalkSAT
{
    private readonly Random random;

    public WalkSAT() : this(new Random())
    {
    }

    public WalkSAT(Random random)
    {
        this.random = random ?? throw new ArgumentNullException(nameof(random));
    }

    /// <summary>
    ///     WALKSAT(clauses, p, maxFlips)test
    /// </summary>
    /// <param name="clauses">Set of clauses in propositional logic.</param>
    /// <param name="p">Probability of a random-walk move (0-1).</param>
    /// <param name="maxFlips">
    ///     Maximum number of flips (<0 for infinity).</param>
    /// <returns>A satisfying Model or null if none found.</returns>
    public Model WalkSATAlgorithm(ISet<Clause> clauses, double p, int maxFlips)
    {
        AssertLegalProbability(p);

        var model = RandomAssignmentToSymbolsInClauses(clauses);

        for (var i = 0; i < maxFlips || maxFlips < 0; i++)
        {
            if (model.Satisfies(clauses))
                return model;

            var clause = RandomlySelectFalseClause(clauses, model);

            if (random.NextDouble() < p)
            {
                var symbol = RandomlySelectSymbolFromClause(clause);
                model = model.Flip(symbol);
            }
            else
            {
                model = FlipSymbolMaximizingSatisfied(clause, clauses, model);
            }
        }

        return null;
    }

    #region Supporting Methods

    private void AssertLegalProbability(double p)
    {
        if (p < 0.0 || p > 1.0)
            throw new ArgumentOutOfRangeException(nameof(p),
                "Probability p must be between 0 and 1.");
    }

    private Model RandomAssignmentToSymbolsInClauses(ISet<Clause> clauses)
    {
        var symbols = new HashSet<PropositionSymbol>();
        foreach (var c in clauses)
            symbols.UnionWith(c.Symbols);

        var values = new Dictionary<PropositionSymbol, bool>();
        foreach (var sym in symbols)
            values[sym] = random.Next(2) == 0;

        return new Model(values);
    }

    private Clause RandomlySelectFalseClause(ISet<Clause> clauses, Model model)
    {
        var falseClauses = clauses.Where(c => model.DetermineValue(c) == false)
            .ToList();
        if (!falseClauses.Any())
            return null;

        var idx = random.Next(falseClauses.Count);
        return falseClauses[idx];
    }

    private PropositionSymbol RandomlySelectSymbolFromClause(Clause clause)
    {
        var symbols = clause.Symbols.ToList();
        var idx = random.Next(symbols.Count);
        return symbols[idx];
    }

    private Model FlipSymbolMaximizingSatisfied(
        Clause clause, ISet<Clause> clauses, Model model)
    {
        var bestModel = model;
        var bestScore = -1;

        foreach (var sym in clause.Symbols)
        {
            var candidate = bestModel.Flip(sym);
            var score = clauses.Count(c => candidate.DetermineValue(c));
            if (score > bestScore)
            {
                bestScore = score;
                bestModel = candidate;
                if (bestScore == clauses.Count)
                    break;
            }
        }

        return bestModel;
    }

    #endregion
}