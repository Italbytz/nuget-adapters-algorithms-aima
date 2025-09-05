// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using Italbytz.AI.Problem;
using Italbytz.AI.Util;
using Italbytz.AI.Util.Datastructure;
using Microsoft.Extensions.Logging;

namespace Italbytz.AI.Search.Framework.QSearch;

public class TreeSearch<TState, TAction> : QueueSearch<TState, TAction>
{
    public TreeSearch() : this(new NodeFactory<TState, TAction>())
    {
    }

    protected TreeSearch(NodeFactory<TState, TAction> nodeFactory) : base(
        nodeFactory)
    {
    }

    /// <summary>
    ///     Receives a problem and a queue implementing the search strategy and
    ///     computes a node referencing a goal state, if such a state was found.
    ///     This template method provides a base for tree and graph search
    ///     implementations. It can be customized by overriding some primitive
    ///     operations.
    /// </summary>
    /// <param name="problem">the search problem</param>
    /// <param name="frontier">
    ///     the data structure for nodes that are waiting to be
    ///     expanded
    /// </param>
    /// <returns>
    ///     a node referencing a goal state, if the goal was found, otherwise
    ///     empty
    /// </returns>
    public override INode<TState, TAction>? FindNode(
        IProblem<TState, TAction> problem,
        NodePriorityQueue<TState, TAction> frontier)
    {
        ClearMetrics();
        var root = NodeFactory.CreateNode(problem.InitialState);
        AddToFrontier(frontier, root);
        if (EarlyGoalTest && IsGoal(root, problem)) return root;
        int ipcost = 0;
        while (!IsFrontierEmpty(frontier))
        {
            var node = RemoveFromFrontier(frontier);
            this.Log(LogLevel.Information,
                "Testing " + node.State + " with cost " + node.PathCost);
            ipcost++;
            this.Log(LogLevel.Information,
                                     "\n Sum of expanded nodes " +ipcost);
            if (IsGoal(node, problem)) return node;
            var successors = NodeFactory.GetSuccessors(node, problem);
            foreach (var successor in successors)
            {
                AddToFrontier(frontier, successor);
                if (EarlyGoalTest && IsGoal(successor, problem))
                    return successor;
            }
        }

        return null;
    }

    protected virtual bool IsFrontierEmpty(
        NodePriorityQueue<TState, TAction> frontier)
    {
        return frontier.Count == 0;
    }

    private bool IsGoal(INode<TState, TAction> node,
        IProblem<TState, TAction> problem)
    {
        if (!problem.TestSolution(node)) return false;
        Metrics.Set(MetricPathCost, node.PathCost);
        return true;
    }

    protected virtual INode<TState, TAction> RemoveFromFrontier(
        NodePriorityQueue<TState, TAction> frontier)
    {
        var result = frontier.Dequeue();
        UpdateMetrics(frontier.Count);
        return result;
    }

    protected virtual void AddToFrontier(
        NodePriorityQueue<TState, TAction> frontier,
        INode<TState, TAction> node)
    {
        frontier.Enqueue(node);
        UpdateMetrics(frontier.Count);
    }
}