using System;
using System.Collections.Generic;
using System.Linq;
using Italbytz.AI;
using Italbytz.AI.Problem;
using Italbytz.AI.Search;
using Italbytz.AI.Search.Framework;

namespace Italbytz.Adapters.Algorithms.AI.Search.Informed;

public class
    RecursiveBestFirstSearch<TState, TAction> : ISearchForActions<TState,
    TAction>
{
    public static string METRIC_NODES_EXPANDED = "nodesExpanded";
    public static string METRIC_MAX_RECURSIVE_DEPTH = "maxRecursiveDepth";
    public static string METRIC_PATH_COST = "pathCost";
    private readonly Func<INode<TState, TAction>, double> _evalFn;
    private readonly INodeFactory<TState, TAction> _nodeFactory;
    private readonly bool avoidLoops;
    private readonly HashSet<TState> explored = new();

    public RecursiveBestFirstSearch(Func<INode<TState, TAction>, double> evalFn)
        : this(evalFn, false)
    {
    }

    public RecursiveBestFirstSearch(Func<INode<TState, TAction>, double> evalFn,
        bool avoidloops) : this(evalFn, avoidloops,
        new NodeFactory<TState, TAction>())
    {
    }

    public RecursiveBestFirstSearch(Func<INode<TState, TAction>, double> evalFn,
        bool avoidloops, NodeFactory<TState, TAction> nodeFactory)
    {
        avoidLoops = avoidloops;
        _nodeFactory = nodeFactory;
        _evalFn = CreateEvalFn(evalFn);
        AddNodeListener(node => Metrics.IncrementInt(METRIC_NODES_EXPANDED));
        Metrics = new Metrics();
    }

    public IMetrics Metrics { get; set; }

    public IEnumerable<TAction>? FindActions(IProblem<TState, TAction> problem)
    {
        explored.Clear();
        clearMetrics();
        var node = _nodeFactory.CreateNode(problem.InitialState);
        var searchresult =
            rbfs(problem, node, _evalFn(node), double.MaxValue, 0);
        if (searchresult.hasSolution())
        {
            var solution = searchresult.getSolutionNode();
            Metrics.Set(METRIC_PATH_COST, solution.PathCost);
            return SearchUtils.ToActions(solution);
        }

        return new LinkedList<TAction>();
    }

    public void AddNodeListener(Action<INode<TState, TAction>> listener)
    {
        _nodeFactory.AddNodeListener(listener);
    }

    private SearchResult<TState, TAction> rbfs(
        IProblem<TState, TAction> problem, INode<TState, TAction> node,
        double node_f, double fCostLimit, int recursiveDepth)
    {
        updateMetrics(recursiveDepth);

        if (problem.TestSolution(node))
            return getResult(null, node, fCostLimit);

        var successors = expandNode(node, problem);

        if (successors.Count == 0 || successors == null)
            return getResult(node, null, double.MaxValue);

        var f = new double[successors.Count];

        for (var i = 0; i < successors.Count; i++)
            f[i] = Math.Max(_evalFn(successors.ElementAt(i)), node_f);

        while (true)
        {
            var bestIndex = getBestFValueInde(f);
            if (f[bestIndex] > fCostLimit)
                return getResult(node, null, f[bestIndex]);
            var altIndex = getNextBestFValueIndex(f, bestIndex);
            var sResult = rbfs(problem, successors.ElementAt(bestIndex),
                f[bestIndex], Math.Min(fCostLimit, f[altIndex]),
                recursiveDepth + 1);
            f[bestIndex] = sResult.getFCostLimit();
            if (sResult.hasSolution())
                return getResult(node, sResult.getSolutionNode(),
                    sResult.getFCostLimit());
        }
    }

    private void updateMetrics(int recursiveDepth)
    {
        var maxRecusriveDepth = Metrics.GetInt(METRIC_MAX_RECURSIVE_DEPTH);
        if (recursiveDepth > maxRecusriveDepth)
            Metrics.Set(METRIC_MAX_RECURSIVE_DEPTH, recursiveDepth);
    }

    private void clearMetrics()
    {
        Metrics.Set(METRIC_PATH_COST, 0);
        Metrics.Set(METRIC_NODES_EXPANDED, 0);
        Metrics.Set(METRIC_MAX_RECURSIVE_DEPTH, 0);
    }

    private int getNextBestFValueIndex(double[] f, int bestIndex)
    {
        var lidx = bestIndex;
        var lowestSoFar = double.MaxValue;
        for (var i = 0; i < f.Length; i++)
            if (i != bestIndex && f[i] < lowestSoFar)
            {
                lowestSoFar = f[i];
                lidx = i;
            }

        return lidx;
    }

    private int getBestFValueInde(double[] f)
    {
        var lidx = 0;
        var lowestSoFar = double.MaxValue;
        for (var i = 0; i < f.Length; i++)
            if (f[i] < lowestSoFar)
            {
                lowestSoFar = f[i];
                lidx = i;
            }

        return lidx;
    }

    private List<INode<TState, TAction>> expandNode(INode<TState, TAction> node,
        IProblem<TState, TAction> problem)
    {
        var res = _nodeFactory.GetSuccessors(node, problem);
        if (avoidLoops)
        {
            explored.Add(node.State);
            res = res.Where(n => !explored.Contains(n.State)).ToList();
        }

        return res;
    }

    private SearchResult<TState, TAction> getResult(
        INode<TState, TAction> currentNode, INode<TState, TAction> solutionNode,
        double fCostLimit)
    {
        if (avoidLoops && currentNode != null)
            explored.Remove(currentNode.State);
        return new SearchResult<TState, TAction>(solutionNode, fCostLimit);
    }

    private static Func<INode<TState, TAction>, double> CreateEvalFn(
        Func<INode<TState, TAction>, double> heuristicFn)
    {
        return node => node.PathCost + heuristicFn(node);
    }

    private class SearchResult<TState, TAction>
    {
        private readonly double fCostLimit;
        private readonly INode<TState, TAction> solutionNode;

        public SearchResult(INode<TState, TAction> sNode, double fCLimit)
        {
            solutionNode = sNode;
            fCostLimit = fCLimit;
        }

        public bool hasSolution()
        {
            return solutionNode != null;
        }

        public INode<TState, TAction> getSolutionNode()
        {
            return solutionNode;
        }

        public double getFCostLimit()
        {
            return fCostLimit;
        }
    }
}