using System.Collections.Generic;

namespace Italbytz.AI.Logic.Planning;

public class PlanningProblemFactory
{
    public static IPlanningProblem GoHomeToSfoProblem()
    {
        var initialState = new State("At(Home)");
        var goal = Utils.Parse("At(SFO)");
        var driveAction = new ActionSchema("Drive", null, "At(Home)",
            "~At(Home)^At(SFOLongTermParking)");
        var shuttleAction = new ActionSchema("Shuttle", null,
            "At(SFOLongTermParking)",
            "~At(SFOLongTermParking)^At(SFO)");
        var taxiAction = new ActionSchema("Taxi", null, "At(Home)",
            "~At(Home)^At(SFO)");
        var problem = new PlanningProblem(initialState, goal, driveAction,
            shuttleAction, taxiAction);
        return problem;
    }

    public static HighLevelAction GetHlaAct(IPlanningProblem problem)
    {
        var refinements = new List<List<IActionSchema>>();
        var act = new HighLevelAction("Act", null, "", "", refinements);
        foreach (var primitiveAction in problem.GetPropositionalisedActions())
            act.Refinements.Add([primitiveAction, act]);
        // ToDo: The precondition is missing
        act.Refinements.Add(new List<IActionSchema>());
        return act;
    }
}