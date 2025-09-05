// The original version of this file is part of <see href="https://github.com/aimacode/aima-csharp"/> which is released under 
// MIT License
// Copyright (c) 2018 aimacode

using System;

namespace Italbytz.AI.Util;

/**
 * Simplified version of
 * <code>java.awt.geom.Point2D</code>
 * . We do not want
 * dependencies to presentation layer packages here.
 *
 * @author R. Lunde
 * @author Mike Stampone
 */
public class Point2D
{
    private readonly double _x;
    private readonly double _y;

    public Point2D(double x, double y)
    {
        _x = x;
        _y = y;
    }

    /**
     * Returns the X coordinate of this
     * <code>Point2D</code>
     * in
     * <code>double</code>
     * precision.
     *
     * @return the X coordinate of this
     * <code>Point2D</code>
     * .
     */
    public double GetX()
    {
        return _x;
    }

    /**
     * Returns the Y coordinate of this
     * <code>Point2D</code>
     * in
     * <code>double</code>
     * precision.
     *
     * @return the Y coordinate of this
     * <code>Point2D</code>
     * .
     */
    public double GetY()
    {
        return _y;
    }

    /**
     * Returns the Euclidean distance between a specified point and this point.
     *
     * @return the Euclidean distance between a specified point and this point.
     */
    public double Distance(Point2D pt)
    {
        // Distance Between X Coordinates
        var xDistance = (pt.GetX() - _x) * (pt.GetX() - _x);
        // Distance Between Y Coordinates
        var yDistance = (pt.GetY() - _y) * (pt.GetY() - _y);
        // Distance Between 2d Points
        var totalDistance = Math.Sqrt(xDistance + yDistance);

        return totalDistance;
    }
}