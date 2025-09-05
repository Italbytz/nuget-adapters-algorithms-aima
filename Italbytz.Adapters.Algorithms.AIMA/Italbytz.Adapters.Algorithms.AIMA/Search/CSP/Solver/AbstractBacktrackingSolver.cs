using System.Collections.Generic;
using Italbytz.AI.Search.CSP.Solver.Inference;

namespace Italbytz.AI.Search.CSP.Solver;

public abstract class
    AbstractBacktrackingSolver<TVar, TVal> : AbstractCspSolver<TVar, TVal>
    where TVar : IVariable
{
    public override IAssignment<TVar, TVal>? Solve(ICSP<TVar, TVal> csp)
    {
        return Backtrack(csp, new Assignment<TVar, TVal>());
    }

    private IAssignment<TVar, TVal>? Backtrack(ICSP<TVar, TVal> csp,
        Assignment<TVar, TVal> assignment)
    {
        IAssignment<TVar, TVal>? result = null;
        if (assignment.IsComplete(csp.Variables)) return assignment;

        var variable = SelectUnassignedVariable(csp, assignment);
        foreach (var value in OrderDomainValues(csp, variable, assignment))
        {
            assignment.Add(variable, value);
            if (assignment.IsConsistent(csp.GetConstraints(variable)))
            {
                var log = Infer(csp, assignment, variable);
                if (!log.InconsistencyFound)
                {
                    result = Backtrack(csp, assignment);
                    if (result != null)
                        break;
                }

                log.Undo(csp);
            }

            assignment.Remove(variable);
        }

        return result;
    }

    protected abstract IInferenceLog<TVar, TVal> Infer(ICSP<TVar, TVal> csp,
        IAssignment<TVar, TVal> assignment, TVar variable);

    protected abstract IEnumerable<TVal> OrderDomainValues(ICSP<TVar, TVal> csp,
        TVar variable, IAssignment<TVar, TVal> assignment);

    protected abstract TVar SelectUnassignedVariable(ICSP<TVar, TVal> csp,
        IAssignment<TVar, TVal> assignment);
}