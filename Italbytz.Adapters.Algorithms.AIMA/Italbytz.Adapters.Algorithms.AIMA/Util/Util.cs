// The original version of this file is part of <see href="https://github.com/aimacode/aima-csharp"/> which is released under 
// MIT License
// Copyright (c) 2018 aimacode

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Italbytz.AI.Util;

/**
 * @author Ravi Mohan
 */
public static class Util
{
    public const string No = "No";

    public const string Yes = "Yes";

    private static readonly Random R = new();

    /**
     * * Get the first element from a list.
     * *
     * * @param l
     * *            the list the first element is to be extracted from.
     * * @return the first element of the passed in list.
     */
    public static T First<T>(List<T> l)
    {
        return l[0];
    }

    /**
     * Get a sublist of all of the elements in the list except for first.
     *
     * @param l
     * the list the rest of the elements are to be extracted from.
     * @return a list of all of the elements in the passed in list except for
     * the first element.
     */
    public static List<T> Rest<T>(List<T> l)
    {
        var newList = l.GetRange(1, l.Count - 1);
        return newList;
    }

    /**
     * Create a set for the provided values.
     * @param values
     * the sets initial values.
     * @return a Set of the provided values.
     */
    public static HashSet<T> CreateSet<T>(params T[] values)
    {
        var set = new HashSet<T>();

        foreach (var t in values) set.Add(t);

        return set;
    }


    public static bool Randombool()
    {
        var trueOrFalse = R.Next(2);
        return trueOrFalse != 0;
    }

    /**
     * Randomly select an element from a list.
     *
     * @param T
     * the type of element to be returned from the list l.
     * @param l
     * a list of type T from which an element is to be selected
     * randomly.
     * @return a randomly selected element from l.
     */
    public static T SelectRandomlyFromList<T>(List<T> l)
    {
        return l[R.Next(l.Count)];
    }


    private static double[] Normalize(IReadOnlyList<double> probDist)
    {
        var len = probDist.Count;
        var total = probDist.Aggregate(0.0, (current, d) => current + d);

        var normalized = new double[len];
        if (total == 0) return normalized;
        for (var i = 0; i < len; i++)
            normalized[i] = probDist[i] / total;
        return normalized;
    }

    public static List<double> Normalize(List<double> values)
    {
        var valuesAsArray = new double[values.Count];
        for (var i = 0; i < valuesAsArray.Length; i++)
            valuesAsArray[i] = values[i];
        var normalized = Normalize(valuesAsArray);
        return normalized.ToList();
    }

    private static int Min(int i, int j)
    {
        return i > j ? j : i;
    }

    private static int Max(int i, int j)
    {
        return i < j ? j : i;
    }

    public static int Max(int i, int j, int k)
    {
        return Max(Max(i, j), k);
    }

    public static int Min(int i, int j, int k)
    {
        return Min(Min(i, j), k);
    }

    public static T? Mode<T>(List<T> l) where T : notnull
    {
        var hash = new Dictionary<T, int>();
        foreach (var obj in l)
            if (hash.ContainsKey(obj))
                hash[obj] = hash[obj] + 1;
            else
                hash.Add(obj, 1);

        var maxkey = hash.Keys.FirstOrDefault();
        foreach (var key in hash.Keys.Where(key =>
                     maxkey != null && hash[key] > hash[maxkey]))
            maxkey = key;

        return maxkey;
    }

    public static string[] Yesno()
    {
        return new[] { Yes, No };
    }

    private static double Log2(double d)
    {
        return Math.Log(d) / Math.Log(2);
    }

    public static double Information(IEnumerable<double> probabilities)
    {
        return probabilities.Sum(d => -1.0 * Log2(d) * d);
    }

    public static List<T> RemoveFrom<T>(IEnumerable<T> list, T member)
    {
        var newList = new List<T>(list);
        newList.Remove(member);
        return newList;
    }

    public static double SumOfSquares<T>(List<T> list)
    {
        double accum = 0;
        foreach (var item in list)
            accum = accum + Convert.ToDouble(item) * Convert.ToDouble(item);
        return accum;
    }

    public static string Ntimes(string s, int n)
    {
        var buf = new StringBuilder();
        for (var i = 0; i < n; i++) buf.Append(s);
        return buf.ToString();
    }

    public static void CheckForNanOrInfinity(double d)
    {
        if (double.IsNaN(d)) throw new ArgumentException("Not a Number");
        if (double.IsInfinity(d))
            throw new ArgumentException("Infinite Number");
    }

    public static int RandomNumberBetween(int i, int j)
    {
        /* i,j bothinclusive */
        return R.Next(j - i + 1) + i;
    }

    public static double CalculateMean(List<double> lst)
    {
        var sum = 0.0;
        foreach (var d in lst) sum = sum + d;
        return sum / lst.Count;
    }

    public static double CalculateStDev(List<double> values, double mean)
    {
        var listSize = values.Count;

        var sumOfDiffSquared = 0.0;
        foreach (var value in values)
        {
            var diffFromMean = value - mean;
            sumOfDiffSquared +=
                diffFromMean * diffFromMean / (listSize - 1);
            // division moved here to avoid sum becoming too big if this
            // doesn't work use incremental formulation
        }

        var variance = sumOfDiffSquared;
        // (listSize - 1);
        // assumes at least 2 members in list.
        return Math.Sqrt(variance);
    }

    public static List<double> NormalizeFromMeanAndStdev(
        IEnumerable<double> values, double mean, double stdev)
    {
        return values.Select(d => (d - mean) / stdev).ToList();
    }

    public static double GenerateRandomDoubleBetween(double lowerLimit,
        double upperLimit)
    {
        return lowerLimit + (upperLimit - lowerLimit) * R.NextDouble();
    }
}