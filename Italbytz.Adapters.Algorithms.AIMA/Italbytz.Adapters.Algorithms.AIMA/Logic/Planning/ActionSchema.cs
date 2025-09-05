using System.Collections.Generic;
using System.Linq;
using Italbytz.AI.Logic.Fol.Kb.Data;
using Italbytz.AI.Logic.Fol.Parsing.Ast;

namespace Italbytz.AI.Logic.Planning;

public class ActionSchema : IActionSchema
{
    private readonly IList<ILiteral> _effectsNegativeLiterals;
    private readonly IList<ILiteral> _effectsPositiveLiterals;

    public ActionSchema(string name, List<ITerm> variables,
        string precondition,
        string effect) : this(name, variables, Utils.Parse(precondition),
        Utils.Parse(effect))
    {
    }

    private ActionSchema(string name, List<ITerm>? variables,
        IList<ILiteral> precondition, IList<ILiteral> effect)
    {
        variables ??= new List<ITerm>();
        Variables = variables;
        Name = name;
        Precondition = precondition;
        Effect = effect;
        _effectsNegativeLiterals = new List<ILiteral>();
        _effectsPositiveLiterals = new List<ILiteral>();
        SortEffects();
    }

    public string Name { get; }

    public IList<ILiteral> Effect { get; }

    public IList<ILiteral> Precondition { get; }

    public List<ITerm> Variables { get; }

    public bool Equals(IActionSchema? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        var equality = Name == ((IActionSchema)other).Name &&
                       Variables.SequenceEqual(other.Variables) &&
                       Precondition.SequenceEqual(other.Precondition) &&
                       Effect.SequenceEqual(other.Effect);
        return equality;
    }

    private void SortEffects()
    {
        foreach (var literal in Effect)
            if (literal.NegativeLiteral)
                _effectsNegativeLiterals.Add(literal);
            else
                _effectsPositiveLiterals.Add(literal);
    }

    public IActionSchema GetActionBySubstitution(
        IEnumerable<IConstant> constants)
    {
        var newPrecondition = new List<ILiteral>();
        var newEffect = new List<ILiteral>();
        foreach (var pre in Precondition)
        {
            var newTerms = new List<ITerm>();
            foreach (var term in pre.Atom.Args)
                if (term is Variable variable)
                {
                    var index = Variables.LastIndexOf(variable);
                    var constant =
                        constants.ElementAt(index);
                    newTerms.Add(constant);
                }
                else
                {
                    newTerms.Add(term);
                }

            newPrecondition.Add(new Literal(
                new Predicate(pre.Atom.SymbolicName, newTerms),
                pre.NegativeLiteral));
        }

        foreach (var eff in Effect)
        {
            var newTerms = new List<ITerm>();
            foreach (var term in eff.Atom.Args)
                if (term is Variable variable)
                {
                    var index = Variables.LastIndexOf(variable);
                    var constant =
                        constants.ElementAt(index);
                    newTerms.Add(constant);
                }
                else
                {
                    newTerms.Add(term);
                }

            newEffect.Add(new Literal(
                new Predicate(eff.Atom.SymbolicName, newTerms),
                eff.NegativeLiteral));
        }

        return new ActionSchema(Name, new List<ITerm>(constants),
            newPrecondition, newEffect);
    }

    public IList<IConstant> GetConstants()
    {
        var constants = new List<IConstant>();
        foreach (var constant in ExtractConstant(Precondition))
            if (!constants.Contains(constant))
                constants.Add(constant);

        foreach (var constant in ExtractConstant(Effect))
            if (!constants.Contains(constant))
                constants.Add(constant);
        return constants;
    }

    private IList<IConstant> ExtractConstant(IList<ILiteral> list)
    {
        var result = new List<IConstant>();
        foreach (var literal in list)
        foreach (var term in literal.Atom.Args)
            if (term is IConstant constant)
                if (!result.Contains(term))
                    result.Add(constant);
        return result;
    }
}