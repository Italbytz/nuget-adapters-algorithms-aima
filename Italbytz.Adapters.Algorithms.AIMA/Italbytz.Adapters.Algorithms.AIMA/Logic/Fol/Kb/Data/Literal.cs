using System.Linq;
using System.Text;
using Italbytz.AI.Logic.Fol.Parsing.Ast;

namespace Italbytz.AI.Logic.Fol.Kb.Data;

public class Literal : ILiteral
{
    private string? strRep;

    public Literal(IAtomicSentence atom, bool negated)
    {
        Atom = atom;
        NegativeLiteral = negated;
    }

    public bool PositiveLiteral => !NegativeLiteral;
    public IAtomicSentence Atom { get; }

    public ILiteral GetComplementaryLiteral()
    {
        return new Literal(Atom, !NegativeLiteral);
    }

    public bool NegativeLiteral { get; }

    public bool Equals(ILiteral? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return NegativeLiteral == other.NegativeLiteral &&
               Atom.SymbolicName.Equals(other.Atom.SymbolicName) &&
               Atom.Args.SequenceEqual(other.Atom.Args);
    }

    public override string ToString()
    {
        if (strRep != null) return strRep;
        var sb = new StringBuilder();
        if (NegativeLiteral) sb.Append('~');
        sb.Append(Atom);
        strRep = sb.ToString();
        return strRep;
    }
}