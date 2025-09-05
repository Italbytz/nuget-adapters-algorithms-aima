using System;
using System.Collections.Generic;
using System.Linq;

namespace Italbytz.AI.Learning.Framework;

public class DataSetFactory
{
    public IDataSet FromFile(string filename, DataSetSpecification spec,
        string seperator)
    {
        throw new NotImplementedException();
    }

    public static IDataSet FromString(string data, DataSetSpecification spec,
        string separator)
    {
        var dataSet = new DataSet(spec);
        var lines = data.Split('\n');
        foreach (var line in lines)
        {
            if (line.Length == 0) continue;
            dataSet.Examples.Add(ExampleFromString(line, spec, separator));
        }

        return dataSet;
    }

    private static IExample ExampleFromString(string data,
        IDataSetSpecification dataSetSpec, string separator)
    {
        var attributes = new Dictionary<string, IAttribute>();
        var attributeValues = data.Split(separator,
            StringSplitOptions.TrimEntries |
            StringSplitOptions.RemoveEmptyEntries);
        if (attributeValues.Length < 11) Console.Write("WTF");
        if (dataSetSpec.IsValid(attributeValues))
        {
            var nw = dataSetSpec.GetAttributeNames()
                .Zip(attributeValues, Tuple.Create);
            foreach (var (name, value) in nw)
            {
                var attributeSpec = dataSetSpec.GetAttributeSpecFor(name);
                var attribute = attributeSpec.CreateAttribute(value);
                attributes.Add(name, attribute);
            }

            var targetAttributeName = dataSetSpec.TargetAttribute;
            return new Example(attributes, attributes[targetAttributeName]);
        }

        throw new SystemException(
            $"Unable to construct Example from {data}");
    }
}