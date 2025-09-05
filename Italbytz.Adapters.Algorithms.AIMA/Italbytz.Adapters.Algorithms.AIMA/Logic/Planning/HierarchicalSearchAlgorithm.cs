using System.Collections.Generic;
using System.Linq;

namespace Italbytz.AI.Logic.Planning;

public class HierarchicalSearchAlgorithm
{
    public IEnumerable<IActionSchema>? HierarchicalSearch(
        IPlanningProblem problem)
    {
        // frontier ← a FIFO queue with [Act] as the only element
        LinkedList<List<IActionSchema>> frontier = new();
        frontier.AddLast([PlanningProblemFactory.GetHlaAct(problem)]);
        while (true)
        {
            // if EMPTY?(frontier) then return failure
            if (frontier.Count == 0)
                return null;
            // plan ← POP(frontier) /* chooses the shallowest plan in frontier */
            var plan = frontier.First.Value;
            frontier.RemoveFirst();
            // hla ← the first HLA in plan, or null if none
            var i = 0;
            IActionSchema? hla;
            while (i < plan.Count && (hla = plan[i]) is not HighLevelAction)
                i++;
            hla = i < plan.Count ? plan[i] : null;
            // prefix,suffix ← the action subsequences before and after hla in plan
            var prefix = new List<IActionSchema>();
            var suffix = new List<IActionSchema>();
            for (var j = 0; j < i; j++)
                prefix.Add(plan[j]);
            for (var j = i + 1; j < plan.Count; j++) suffix.Add(plan[j]);
            // outcome ← RESULT(problem.INITIAL-STATE, prefix)
            var outcome = problem.InitialState.Result(prefix);
            // if hla is null then /* so plan is primitive and outcome is its result */
            if (hla == null)
            {
                // if outcome satisfies problem.GOAL then return plan
                if (problem.Goal.All(goal => outcome.Fluents.Contains(goal)))
                    return plan;
            }
            else
            {
                // else for each sequence in REFINEMENTS(hla, outcome, hierarchy) do
                foreach (var refinement in Refinements(hla, outcome))
                {
                    // frontier ← INSERT(APPEND(prefix, sequence, suffix), frontier)
                    var newPlan = new List<IActionSchema>();
                    newPlan.AddRange(prefix);
                    newPlan.AddRange(refinement);
                    newPlan.AddRange(suffix);
                    frontier.AddLast(newPlan);
                }
            }
        }
    }

    private IEnumerable<IEnumerable<IActionSchema>> Refinements(
        IActionSchema hla, IState outcome)
    {
        var refinements = new List<List<IActionSchema>>();
        var hlarefinements = ((HighLevelAction)hla).Refinements;
        foreach (var refinement in hlarefinements)
            /*    if (refinement.Count > 0)
                    refinements.Add(refinement);*/
            if (refinement.Count > 0)
            {
                if (outcome.IsApplicable(refinement[0]))
                    refinements.Add(refinement);
            }
            else
            {
                refinements.Add(refinement);
            }

        return refinements;
    }
}