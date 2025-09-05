namespace Italbytz.AI.Search.CSP.Solver;

public abstract class AbstractCspSolver<TVar, TVal> : ICspSolver<TVar, TVal>
    where TVar : IVariable
{
    public abstract IAssignment<TVar, TVal>? Solve(ICSP<TVar, TVal> csp);
}