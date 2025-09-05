namespace Italbytz.AI.Learning.Framework;

/// <inheritdoc cref="IAttribute" />
public class StringAttribute : IAttribute
{
    private readonly IAttributeSpecification _spec;
    private readonly string _value;

    public StringAttribute(string value, IAttributeSpecification spec)
    {
        _value = value;
        _spec = spec;
    }


    public string Name()
    {
        return _spec.AttributeName.Trim();
    }

    public string ValueAsString()
    {
        return _value.Trim();
    }
}