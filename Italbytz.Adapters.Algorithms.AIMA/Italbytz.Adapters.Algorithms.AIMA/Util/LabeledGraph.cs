// The original version of this file is part of <see href="https://github.com/aimacode/aima-csharp"/> which is released under 
// MIT License
// Copyright (c) 2018 aimacode

using System.Collections.Generic;

namespace Italbytz.AI.Util;

/**
 * Represents a directed labeled graph. Vertices are represented by their unique
 * labels and labeled edges by means of nested hashtables. Variant of class
 * {@code aima.util.Table}. This version is more dynamic, it requires no
 * initialization and can add new items whenever needed.
 *
 * @author R. Lunde
 * @author Mike Stampone
 */
public class LabeledGraph<TVertexLabel, TEdgeLabel>
{
    /**
     * List of the labels of all vertices within the graph.
     */
    private readonly List<TVertexLabel> _vertexLabels;

    /**
     * Lookup for edge label information. Contains an entry for every vertex
     * label.
     */
    private readonly
        Dictionary<TVertexLabel, Dictionary<TVertexLabel, TEdgeLabel>>
        globalEdgeLookup;

    /**
     * Creates a new empty graph.
     */
    public LabeledGraph()
    {
        globalEdgeLookup =
            new Dictionary<TVertexLabel,
                Dictionary<TVertexLabel, TEdgeLabel>>();
        _vertexLabels = new List<TVertexLabel>();
    }

    /**
     * Adds a new vertex to the graph if it is not already present.
     *
     * @param v
     * the vertex to add
     */
    public void addVertex(TVertexLabel v)
    {
        checkForNewVertex(v);
    }

    /**
     * Adds a directed labeled edge to the graph. The end points of the edge are
     * specified by vertex labels. New vertices are automatically identified and
     * added to the graph.
     *
     * @param from
     * the first vertex of the edge
     * @param to
     * the second vertex of the edge
     * @param el
     * an edge label
     */
    public void set(TVertexLabel from, TVertexLabel to, TEdgeLabel el)
    {
        var localEdgeLookup = checkForNewVertex(from);
        localEdgeLookup.Add(to, el);
        checkForNewVertex(to);
    }

    /**
     * Handles new vertices.
     */
    private Dictionary<TVertexLabel, TEdgeLabel> checkForNewVertex(
        TVertexLabel v)
    {
        Dictionary<TVertexLabel, TEdgeLabel>? result = null;
        try
        {
            result = globalEdgeLookup[v];
        }
        catch (KeyNotFoundException e)
        {
        }

        if (result == null)
        {
            result = new Dictionary<TVertexLabel, TEdgeLabel>();
            globalEdgeLookup.Add(v, result);
            _vertexLabels.Add(v);
        }

        return result;
    }

    /**
     * Removes an edge from the graph.
     *
     * @param from
     * the first vertex of the edge
     * @param to
     * the second vertex of the edge
     */
    public void remove(TVertexLabel from, TVertexLabel to)
    {
        var localEdgeLookup = globalEdgeLookup[from];
        if (localEdgeLookup != null)
            localEdgeLookup.Remove(to);
    }

    /**
     * Returns the label of the edge between the specified vertices, or null if
     * there is no edge between them.
     *
     * @param from
     * the first vertex of the ege
     * @param to
     * the second vertex of the edge
     *
     * @return the label of the edge between the specified vertices, or null if
     * there is no edge between them.
     */
    public TEdgeLabel get(TVertexLabel from, TVertexLabel to)
    {
        var localEdgeLookup = globalEdgeLookup[from];
        return localEdgeLookup == null ? default : localEdgeLookup[to];
    }

    /**
     * Returns the labels of those vertices which can be obtained by following
     * the edges starting at the specified vertex.
     */
    public List<TVertexLabel> getSuccessors(TVertexLabel v)
    {
        var result = new List<TVertexLabel>();
        var localEdgeLookup = globalEdgeLookup[v];
        if (localEdgeLookup != null)
            result.AddRange(localEdgeLookup.Keys);
        return result;
    }

    /**
     * Returns the labels of all vertices within the graph.
     */
    public List<TVertexLabel> getVertexLabels()
    {
        return _vertexLabels;
    }

    /**
     * Checks whether the given label is the label of one of the vertices.
     */
    public bool isVertexLabel(TVertexLabel v)
    {
        return globalEdgeLookup[v] != null;
    }

    /**
     * Removes all vertices and all edges from the graph.
     */
    public void clear()
    {
        _vertexLabels.Clear();
        globalEdgeLookup.Clear();
    }
}