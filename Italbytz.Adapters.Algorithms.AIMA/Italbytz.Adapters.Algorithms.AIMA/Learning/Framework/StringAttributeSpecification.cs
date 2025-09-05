using System.Collections.Generic;
using System.Linq;

namespace Italbytz.AI.Learning.Framework;

/// <inheritdoc cref="IAttributeSpecification" />
public class StringAttributeSpecification : IAttributeSpecification
{
    public StringAttributeSpecification(string attributeName,
        IEnumerable<string> attributePossibleValues)
    {
        AttributeName = attributeName;
        AttributePossibleValues = attributePossibleValues;
    }

    public IEnumerable<string> AttributePossibleValues { get; }

    public string AttributeName { get; }

    /*public StringAttributeSpecification(string attributeName, IEnumerable<string> attributePossibleValues) : this(attributeName, new List<string>(attributePossibleValues))
    {
    } */

    public IAttribute CreateAttribute(string rawValue)
    {
        return new StringAttribute(rawValue, this);
    }

    public bool IsValid(string value)
    {
        return AttributePossibleValues.Contains(value);
    }
}