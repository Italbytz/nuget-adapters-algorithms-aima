using System.Collections.Generic;
using System.Linq;
using Italbytz.AI.Search.CSP.Solver.Inference;

namespace Italbytz.AI.Search.CSP.Solver;

public class
    FlexibleBacktrackingSolver<TVar, TVal> : AbstractBacktrackingSolver<TVar,
    TVal> where TVar : IVariable
{
    public IInferenceStrategy<TVar, TVal>? inferenceStrategy { get; set; }

    public CspHeuristics.IVariableSelectionStrategy<TVar, TVal>?
        variableSelectionStrategy { get; set; }

    public CspHeuristics.IValueOrderingStrategy<TVar, TVal>?
        valueOrderingStrategy { get; set; }

    public override IAssignment<TVar, TVal>? Solve(ICSP<TVar, TVal> csp)
    {
        if (inferenceStrategy == null) return base.Solve(csp);
        csp = csp.CopyDomains();
        var log = inferenceStrategy.Apply(csp);
        return log is { IsEmpty: false, InconsistencyFound: true }
            ? null
            : base.Solve(csp);
    }

    protected override IInferenceLog<TVar, TVal> Infer(ICSP<TVar, TVal> csp,
        IAssignment<TVar, TVal> assignment, TVar variable)
    {
        return inferenceStrategy != null
            ? inferenceStrategy.Apply(csp, assignment, variable)
            : new EmptyInferenceLog<TVar, TVal>();
    }

    protected override IEnumerable<TVal> OrderDomainValues(ICSP<TVar, TVal> csp,
        TVar variable, IAssignment<TVar, TVal> assignment)
    {
        return valueOrderingStrategy != null
            ? valueOrderingStrategy.Apply(csp, assignment, variable)
            : csp.GetDomain(variable);
    }

    protected override TVar SelectUnassignedVariable(ICSP<TVar, TVal> csp,
        IAssignment<TVar, TVal> assignment)
    {
        var unassigned = csp.Variables.ToList()
            .Where(v => !assignment.Contains(v)).ToList();
        return variableSelectionStrategy != null
            ? variableSelectionStrategy.Apply(csp, unassigned).First()
            : unassigned.First();
    }
}