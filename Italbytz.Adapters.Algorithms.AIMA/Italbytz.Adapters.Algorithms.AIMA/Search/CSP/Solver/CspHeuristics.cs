using System.Collections.Generic;
using System.Linq;

namespace Italbytz.AI.Search.CSP.Solver;

public class CspHeuristics
{
    public static IVariableSelectionStrategy<TVar, TVal> Mrv<TVar, TVal>()
        where TVar : IVariable
    {
        return new MinimumRemainingValuesHeuristic<TVar, TVal>();
    }

    public static IValueOrderingStrategy<TVar, TVal> Lcv<TVar, TVal>()
        where TVar : IVariable
    {
        return new LeastConstrainingValueHeuristic<TVar, TVal>();
    }

    public static IVariableSelectionStrategy<TVar, TVal> Deg<TVar, TVal>()
        where TVar : IVariable
    {
        return new DegreeHeuristic<TVar, TVal>();
    }

    public static IVariableSelectionStrategy<TVar, TVal> MrvDeg<TVar, TVal>()
        where TVar : IVariable
    {
        return new MrvDegHeuristic<TVar, TVal>();
    }

    public interface IVariableSelectionStrategy<TVar, TVal>
        where TVar : IVariable
    {
        public IList<TVar> Apply(ICSP<TVar, TVal> csp, IList<TVar> vars);
    }

    public interface IValueOrderingStrategy<TVar, TVal> where TVar : IVariable
    {
        public IList<TVal> Apply(ICSP<TVar, TVal> csp,
            IAssignment<TVar, TVal> assignment, TVar var);
    }

    public class
        MinimumRemainingValuesHeuristic<TVar, TVal> : IVariableSelectionStrategy
        <TVar, TVal> where TVar : IVariable
    {
        public IList<TVar> Apply(ICSP<TVar, TVal> csp, IList<TVar> vars)
        {
            var minDomain = int.MaxValue;
            var minDomainVars = new List<TVar>();
            foreach (var var in vars)
            {
                var domainSize = csp.GetDomain(var).Count;
                if (domainSize < minDomain)
                {
                    minDomain = domainSize;
                    minDomainVars.Clear();
                    minDomainVars.Add(var);
                }
                else if (domainSize == minDomain)
                {
                    minDomainVars.Add(var);
                }
            }

            return minDomainVars;
        }
    }

    public class
        DegreeHeuristic<TVar, TVal> : IVariableSelectionStrategy<TVar, TVal>
        where TVar : IVariable
    {
        public IList<TVar> Apply(ICSP<TVar, TVal> csp, IList<TVar> vars)
        {
            var maxDegree = 0;
            var maxDegreeVars = new List<TVar>();
            foreach (var var in vars)
            {
                var degree = 0;
                foreach (var constraint in csp.GetConstraints(var))
                    degree += constraint.Scope.Count;
                if (degree > maxDegree)
                {
                    maxDegree = degree;
                    maxDegreeVars.Clear();
                    maxDegreeVars.Add(var);
                }
                else if (degree == maxDegree)
                {
                    maxDegreeVars.Add(var);
                }
            }

            return maxDegreeVars;
        }
    }

    public class
        MrvDegHeuristic<TVar, TVal> : IVariableSelectionStrategy<TVar, TVal>
        where TVar : IVariable
    {
        public IList<TVar> Apply(ICSP<TVar, TVal> csp, IList<TVar> vars)
        {
            var mrvVars =
                new MinimumRemainingValuesHeuristic<TVar, TVal>().Apply(csp,
                    vars);
            return new DegreeHeuristic<TVar, TVal>().Apply(csp, mrvVars);
        }
    }

    public class
        LeastConstrainingValueHeuristic<TVar, TVal> : IValueOrderingStrategy<
        TVar, TVal> where TVar : IVariable
    {
        public IList<TVal> Apply(ICSP<TVar, TVal> csp,
            IAssignment<TVar, TVal> assignment, TVar var)
        {
            var values = csp.GetDomain(var);
            var valueScores = new Dictionary<TVal, int>();
            foreach (var value in values)
            {
                var score = 0;
                assignment.Add(var, value);
                foreach (var constraint in csp.GetConstraints(var))
                {
                    if (!constraint.Scope.Contains(var)) continue;
                    foreach (var otherVar in constraint.Scope)
                    {
                        if (otherVar.Equals(var)) continue;

                        if (assignment.Contains(otherVar)) continue;
                        foreach (var otherValue in csp.GetDomain(otherVar))
                        {
                            assignment.Add(otherVar, otherValue);
                            if (constraint.IsSatisfiedWith(assignment)) score++;
                            assignment.Remove(otherVar);
                        }
                    }
                }

                assignment.Remove(var);
                valueScores.Add(value, score);
            }

            return values.OrderBy(value => valueScores[value]).ToList();
        }
    }
}