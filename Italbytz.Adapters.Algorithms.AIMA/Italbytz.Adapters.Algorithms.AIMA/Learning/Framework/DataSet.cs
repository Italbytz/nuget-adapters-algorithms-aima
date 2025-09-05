using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Italbytz.AI.Learning.Framework;

/// <inheritdoc cref="IDataSet" />
public class DataSet : IDataSet
{
    public DataSet(IDataSetSpecification spec)
    {
        Examples = new List<IExample>();
        Specification = spec;
    }

    public IEnumerator<IExample> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public List<IExample> Examples { get; set; }
    public IDataSetSpecification Specification { get; set; }

    public IEnumerable<string> GetNonTargetAttributes()
    {
        return Util.Util.RemoveFrom(Specification.GetAttributeNames(),
            Specification.TargetAttribute);
    }

    public IEnumerable<string> GetPossibleAttributeValues(string attributeName)
    {
        return Specification.GetPossibleAttributeValues(attributeName);
    }

    public IDataSet EmptyDataSet()
    {
        return new DataSet(Specification);
    }

    public double CalculateGainFor(string parameterName)
    {
        var dict = SplitByAttribute(parameterName);
        var totalSize = Examples.Count;
        var remainder = 0.0;
        foreach (var parameterValue in dict.Keys)
        {
            var reducedDataSetSize = dict[parameterValue].Examples.Count;
            var information = dict[parameterValue].GetInformationFor();
            remainder += (double)reducedDataSetSize / totalSize *
                         information;
        }

        return GetInformationFor() - remainder;
    }

    public double GetInformationFor()
    {
        var attributeName = Specification.TargetAttribute;
        var counts = new Dictionary<string, int>();
        foreach (var val in Examples.Select(e =>
                     e.GetAttributeValueAsString(attributeName)))
            if (!counts.TryAdd(val, 1))
                counts[val] = counts[val] + 1;

        var data = counts.Values.ToList().ConvertAll(Convert.ToDouble);

        data = Util.Util.Normalize(data);

        return Util.Util.Information(data);
    }

    public IDataSet MatchingDataSet(string attributeName, string attributeValue)
    {
        var ds = new DataSet(Specification);
        foreach (var e in Examples.Where(e => e
                     .GetAttributeValueAsString(attributeName).Equals(
                         attributeValue)))
            ds.Examples.Add(e);
        return ds;
    }

    private Dictionary<string, IDataSet> SplitByAttribute(string parameterName)
    {
        var dict = new Dictionary<string, IDataSet>();
        foreach (var e in Examples)
        {
            var val = e.GetAttributeValueAsString(parameterName);
            if (dict.ContainsKey(val))
            {
                dict[val].Examples.Add(e);
            }
            else
            {
                var ds = new DataSet(Specification);
                ds.Examples.Add(e);
                dict.Add(val, ds);
            }
        }

        return dict;
    }
}