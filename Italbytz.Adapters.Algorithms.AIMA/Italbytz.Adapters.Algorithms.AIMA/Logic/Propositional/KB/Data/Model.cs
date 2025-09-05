using System;
using System.Collections.Generic;
using Italbytz.AI.Logic.Propositional.Parsing.Ast;

namespace Italbytz.AI.Logic.Propositional.KB.Data;

public class Model(Dictionary<PropositionSymbol, bool> values)
{
    public bool Satisfies(ISet<Clause> clauses)
    {
        throw new NotImplementedException();
    }

    public Model Flip(PropositionSymbol symbol)
    {
        throw new NotImplementedException();
    }

    public bool DetermineValue(Clause clause)
    {
        throw new NotImplementedException();
    }
}