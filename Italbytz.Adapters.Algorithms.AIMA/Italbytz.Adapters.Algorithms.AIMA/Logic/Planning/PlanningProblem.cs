using System.Collections.Generic;
using Italbytz.AI.Logic.Fol.Kb.Data;
using Italbytz.AI.Logic.Fol.Parsing.Ast;
using Italbytz.AI.Util.MathUtils.Permute;

namespace Italbytz.AI.Logic.Planning;

public class PlanningProblem : IPlanningProblem
{
    private readonly ISet<ActionSchema> _actionSchemas;
    private IList<IActionSchema> _propositionalisedActionSchemas;

    public PlanningProblem(State initialState, IList<ILiteral> goal,
        ISet<ActionSchema> actionSchemas)
    {
        InitialState = initialState;
        _actionSchemas = actionSchemas;
        Goal = goal;
    }

    public PlanningProblem(State initialState, IList<ILiteral> goal,
        params ActionSchema[] actions) : this(initialState, goal,
        new HashSet<ActionSchema>(actions))
    {
    }

    public IList<ILiteral> Goal { get; }

    public IState InitialState { get; }

    public IEnumerable<IActionSchema> GetPropositionalisedActions()
    {
        if (_propositionalisedActionSchemas == null)
        {
            var problemConstants = GetProblemConstants();
            _propositionalisedActionSchemas = new List<IActionSchema>();
            foreach (var actionSchema in _actionSchemas)
            {
                var numberOfVars = actionSchema.Variables.Count;

                foreach (var constants in
                         PermutationGenerator.GeneratePermutations(
                             problemConstants, numberOfVars))
                    _propositionalisedActionSchemas.Add(
                        actionSchema.GetActionBySubstitution(constants));
            }
        }

        return _propositionalisedActionSchemas;
    }

    private IList<IConstant> GetProblemConstants()
    {
        var constants = new List<IConstant>();
        foreach (var literal in InitialState.Fluents)
        foreach (var term in literal.Atom.Args)
            if (term is IConstant constant)
                if (!constants.Contains(constant))
                    constants.Add(constant);

        foreach (var literal in Goal)
        {
            foreach (var term in literal.Atom.Args)
                if (term is IConstant constant)
                    if (!constants.Contains(constant))
                        constants.Add(constant);
            {
            }
        }

        foreach (var actionSchema in _actionSchemas)
        foreach (var constant in actionSchema.GetConstants())
            if (!constants.Contains(constant))
                constants.Add(constant);

        return constants;
    }
}