// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using Italbytz.AI.Search;
using Italbytz.AI.Util.Datastructure;

namespace Italbytz.AI.Tests.Environment.NQueens;

public static class NQueensFunctions
{
    public static double GetNumberOfAttackingPairs(
        INode<NQueensBoard, QueenAction> node)
    {
        return node.State.GetNumberOfAttackingPairs();
    }

    internal static List<QueenAction> GetCSFActions(NQueensBoard state)
    {
        var actions = new List<QueenAction>();
        for (var i = 0; i < state.Size; i++)
        for (var j = 0; j < state.Size; j++)
        {
            var loc = new XYLocation(i, j);
            if (!state.QueenExistsAt(loc))
                actions.Add(new QueenAction(QueenAction.MOVE_QUEEN, loc));
        }

        return actions;
    }

    internal static NQueensBoard GetResult(NQueensBoard state,
        QueenAction action)
    {
        var result = new NQueensBoard(state.Size);
        result.SetQueensAt(state.GetQueenPositions());
        switch (action.Name)
        {
            case QueenAction.MOVE_QUEEN:
                result.MoveQueenTo(action.Location);
                break;
            case QueenAction.PLACE_QUEEN:
                result.AddQueenAt(action.Location);
                break;
            case QueenAction.REMOVE_QUEEN:
                result.RemoveQueenFrom(action.Location);
                break;
        }

        return result;
    }

    internal static bool TestGoal(NQueensBoard state)
    {
        return state.GetNumberOfQueensOnBoard() == state.Size &&
               state.GetNumberOfAttackingPairs() == 0;
    }
}