namespace Italbytz.AI.Logic.Fol.Parsing.Ast;

public class Variable : IVariable
{
    public Variable(string s)
    {
        SymbolicName = s.Trim();
    }

    public int Indexical { get; } = -1;

    public string SymbolicName { get; }

    public bool Equals(ITerm? other)
    {
        return other is IVariable variable && Equals(variable);
    }

    public bool Equals(IVariable? other)
    {
        if (other is null) return false;

        return SymbolicName == other.SymbolicName &&
               Indexical == other.Indexical;
    }

    public override string ToString()
    {
        return SymbolicName;
    }
}