using System.Collections.Generic;
using System.Linq;
using Italbytz.AI.Util;

namespace Italbytz.AI.Search.CSP.Solver;

public class MinConflictsSolver<TVar, TVal> : AbstractCspSolver<TVar, TVal>
    where TVar : IVariable
{
    private readonly int _maxSteps;

    public MinConflictsSolver(int maxSteps)
    {
        _maxSteps = maxSteps;
    }

    public override IAssignment<TVar, TVal>? Solve(ICSP<TVar, TVal> csp)
    {
        var current = GenerateRandomAssignment(csp);
        for (var i = 0; i < _maxSteps; i++)
        {
            if (current.IsSolution(csp)) return current;
            var variables = GetConflictedVariables(csp, current);
            var variable =
                variables.ElementAt(
                    ThreadSafeRandomNetCore.LocalRandom.Next(variables.Count));
            var value = GetMinConflictValue(csp, variable, current);
            current.Add(variable, value);
        }

        return null;
    }

    private TVal GetMinConflictValue(ICSP<TVar, TVal> csp, TVar variable,
        IAssignment<TVar, TVal> assignment)
    {
        var constraints = csp.GetConstraints(variable);
        var testAssignment = (IAssignment<TVar, TVal>)assignment.Clone();
        var minConflicts = int.MaxValue;
        var resultCandidates = new List<TVal>();
        foreach (var value in csp.GetDomain(variable))
        {
            testAssignment.Add(variable, value);
            var conflicts = constraints.Count(constraint =>
                !constraint.IsSatisfiedWith(testAssignment));
            if (conflicts < minConflicts)
            {
                minConflicts = conflicts;
                resultCandidates.Clear();
                resultCandidates.Add(value);
            }
            else if (conflicts == minConflicts)
            {
                resultCandidates.Add(value);
            }

            testAssignment.Remove(variable);
        }

        return resultCandidates.ElementAt(
            ThreadSafeRandomNetCore.LocalRandom.Next(resultCandidates.Count));
    }

    private ISet<TVar> GetConflictedVariables(ICSP<TVar, TVal> csp,
        IAssignment<TVar, TVal> current)
    {
        return new HashSet<TVar>(csp.Variables.Where(variable =>
            csp.Constraints.Any(constraint =>
                !constraint.IsSatisfiedWith(current))));
    }

    private TVar SelectVariableToChange(ICSP<TVar, TVal> csp,
        IAssignment<TVar, TVal> assignment)
    {
        var variables =
            csp.Variables.Where(variable => !assignment.Contains(variable));
        var variable =
            variables.ElementAt(
                ThreadSafeRandomNetCore.LocalRandom.Next(variables.Count()));
        return variable;
    }

    private IAssignment<TVar, TVal> GenerateRandomAssignment(
        ICSP<TVar, TVal> csp)
    {
        var assignment = new Assignment<TVar, TVal>();
        foreach (var variable in csp.Variables)
            assignment.Add(variable,
                csp.GetDomain(variable)
                    .ElementAt(
                        ThreadSafeRandomNetCore.LocalRandom.Next(
                            csp.GetDomain(variable).Count)));
        return assignment;
    }
}