namespace Italbytz.AI.Logic.Fol.Parsing.Ast;

public class Constant : IConstant
{
    public Constant(string s)
    {
        Value = s;
    }

    public string Value { get; }

    public string SymbolicName => Value;

    public bool Equals(IConstant? other)
    {
        if (other is null) return false;

        return Value == other.Value;
    }

    public bool Equals(ITerm? other)
    {
        return other is IConstant constant && Equals(constant);
    }

    public override string ToString()
    {
        return Value;
    }
}