// The original version of this file is part of <see href="https://github.com/aimacode/aima-java"/> which is released under
// MIT License
// Copyright (c) 2015 aima-java contributors

using System.Collections.Generic;
using System.Linq;

namespace Italbytz.AI.Search.CSP;

public class Assignment<TVar, TVal> : IAssignment<TVar, TVal>
    where TVar : IVariable
{
    private Dictionary<TVar, TVal> variableToValueMap = new();

    public IEnumerable<TVar> Variables =>
        new List<TVar>(variableToValueMap.Keys);

    public TVal? GetValue(TVar variable)
    {
        return !variableToValueMap.TryGetValue(variable, out var value)
            ? default
            : value;
    }

    public void Add(TVar variable, TVal value)
    {
        variableToValueMap[variable] = value;
    }

    public void Remove(TVar variable)
    {
        variableToValueMap.Remove(variable);
    }

    public bool Contains(TVar variable)
    {
        return variableToValueMap.ContainsKey(variable);
    }

    public bool IsConsistent(IEnumerable<IConstraint<TVar, TVal>> constraints)
    {
        return constraints.All(constraint => constraint.IsSatisfiedWith(this));
    }

    public bool IsComplete(IEnumerable<TVar> variables)
    {
        return variables.All(Contains);
    }

    public bool IsSolution(ICSP<TVar, TVal> csp)
    {
        return IsConsistent(csp.Constraints) && IsComplete(csp.Variables);
    }


    public object Clone()
    {
        IAssignment<TVar, TVal> result;
        result = (IAssignment<TVar, TVal>)MemberwiseClone();
        ((Assignment<TVar, TVal>)result).variableToValueMap =
            new Dictionary<TVar, TVal>(variableToValueMap);
        return result;
    }

    public override string ToString()
    {
        return variableToValueMap.Keys.Aggregate("",
            (current, variable) => current + variable + " = " +
                                   variableToValueMap[variable] + "\n");
    }
}