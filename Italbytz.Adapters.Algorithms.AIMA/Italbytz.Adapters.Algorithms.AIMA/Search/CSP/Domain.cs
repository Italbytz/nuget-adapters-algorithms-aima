using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Italbytz.AI.Search.CSP;

public class Domain<TVal> : IDomain<TVal>
{
    public Domain(IEnumerable<TVal> values)
    {
        Values = values.ToArray();
    }

    public Domain(params TVal[] values)
    {
        Values = values;
    }

    public TVal[] Values { get; }


    public TVal this[int index] => Values[index];

    public IEnumerator<TVal> GetEnumerator()
    {
        return ((IEnumerable<TVal>)Values).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool Equals(IDomain<TVal>? other)
    {
        return other != null && Values.SequenceEqual(other.Values);
    }

    public bool Contains(TVal value)
    {
        return Values.Contains(value);
    }


    public TVal Get(int index)
    {
        return Values[index];
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((object?)(Domain<TVal>)obj);
    }

    public override string ToString()
    {
        return string.Join(", ", Values);
    }

    public override int GetHashCode()
    {
        return Values.GetHashCode();
    }
}