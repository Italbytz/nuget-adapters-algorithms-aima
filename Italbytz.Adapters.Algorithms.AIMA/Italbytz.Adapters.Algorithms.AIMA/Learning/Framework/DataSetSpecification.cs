using System;
using System.Collections.Generic;
using System.Linq;

namespace Italbytz.AI.Learning.Framework;

/// <inheritdoc cref="IDataSetSpecification" />
public class DataSetSpecification : IDataSetSpecification
{
    private readonly List<IAttributeSpecification> _attributeSpecifications;

    public DataSetSpecification()
    {
        _attributeSpecifications = new List<IAttributeSpecification>();
    }

    public string TargetAttribute { get; set; }

    public bool IsValid(IEnumerable<string> uncheckedAttributes)
    {
        var attributes = uncheckedAttributes.ToList();
        if (_attributeSpecifications.Count != attributes.Count)
            throw new SystemException(
                $"size mismatch specsize = {_attributeSpecifications.Count} attributes size = {attributes.Count}");
        return _attributeSpecifications.Zip(attributes, Tuple.Create)
            .All(au => au.Item1.IsValid(au.Item2));
    }

    public IEnumerable<string> GetAttributeNames()
    {
        return _attributeSpecifications.Select(ats => ats.AttributeName)
            .ToList();
    }

    public IAttributeSpecification GetAttributeSpecFor(string name)
    {
        var spec =
            _attributeSpecifications.FirstOrDefault(spec =>
                spec.AttributeName.Equals(name));
        if (spec != null) return spec;
        throw new SystemException($"no attribute spec for {name}");
    }

    public IEnumerable<string> GetPossibleAttributeValues(string attributeName)
    {
        foreach (var ats in _attributeSpecifications)
            if (ats.AttributeName.Equals(attributeName))
                return ((StringAttributeSpecification)ats)
                    .AttributePossibleValues;

        throw new SystemException($"No such attribute {attributeName}");
    }

    public void DefineStringAttribute(string name, string[] attributeValues)
    {
        _attributeSpecifications.Add(new StringAttributeSpecification(name,
            attributeValues));
        TargetAttribute = name; // target defaults to last column added
    }

    public void DefineNumericAttribute(string name)
    {
        _attributeSpecifications.Add(new NumericAttributeSpecification(name));
        TargetAttribute = name; // target defaults to last column added
    }
}