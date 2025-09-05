using System.Collections.Generic;
using Italbytz.AI.Logic.Fol.Kb.Data;
using Italbytz.AI.Logic.Fol.Parsing.Ast;
using Constant = Italbytz.AI.Logic.Fol.Parsing.Ast.Constant;
using Variable = Italbytz.AI.Logic.Fol.Parsing.Ast.Variable;

namespace Italbytz.AI.Logic.Planning;

public class Utils
{
    public static IList<ILiteral> Parse(string s)
    {
        if (string.IsNullOrEmpty(s)) return new List<ILiteral>();
        s = s.Replace(" ", "");
        var tokens = s.Split('^');
        var literals = new List<ILiteral>();
        foreach (var token in tokens)
        {
            var terms = token.Split('(', ')', ',');
            var literalTerms = new List<ITerm>();
            var negated = false;
            for (var i = 1; i < terms.Length; i++)
            {
                var termString = terms[i];
                if (string.IsNullOrEmpty(termString)) continue;
                ITerm term;
                if (termString == termString.ToLower())
                    term = new Variable(termString);
                else
                    term = new Constant(termString);
                literalTerms.Add(term);
            }

            var name = terms[0];
            if (name[0] == '~')
            {
                negated = true;
                name = name[1..];
            }

            var predicate = new Predicate(name, literalTerms);
            var literal = new Literal(predicate, negated);
            literals.Add(literal);
        }

        return literals;
    }
}