using System.Collections.Generic;

namespace Italbytz.AI.Search.Local;

public class Individual<TAlphabet> : IIndividual<TAlphabet>
{
    public Individual(List<TAlphabet> representation)
    {
        Representation = representation;
    }

    public List<TAlphabet> Representation { get; }
    public int Descendants { get; set; }
}