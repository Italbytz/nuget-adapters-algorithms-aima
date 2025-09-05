using System.Collections.Generic;
using Italbytz.AI.Logic.Propositional.Parsing.Ast;

namespace Italbytz.AI.Logic.Propositional.KB.Data;

public class Clause
{
    public IEnumerable<PropositionSymbol> Symbols { get; set; }
}