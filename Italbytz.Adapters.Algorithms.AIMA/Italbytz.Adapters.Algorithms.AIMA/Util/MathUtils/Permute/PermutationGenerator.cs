using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Italbytz.AI.Util.MathUtils.Permute;

public class PermutationGenerator
{
    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(
        IEnumerable<T> list)
    {
        if (list.Count() == 1)
            yield return list;
        else
            foreach (var element in list)
            {
                var remainingList = list.Except(new[] { element });
                foreach (var subPermutation in GetPermutations(remainingList))
                    yield return new[] { element }.Concat(subPermutation);
            }
    }

    public static IEnumerable<IList<T>> GeneratePermutations<T>(IList<T> list,
        int r)
    {
        if (r > list.Count) return Array.Empty<IList<T>>();
        var rFact = Factorial(r);
        var total = (long)CombinationGenerator.Ncr(list.Count, r) * rFact;
        return new PermutationsEnumerable<T>(list, r, rFact, total);
    }

    private static long Factorial(int n)
    {
        var nfact = 1;
        for (var i = 1; i <= n; i++) nfact *= i;
        return nfact;
    }

    public static int[] GenerateNextPermutation(int[] temp, int n)
    {
        var m = n - 1;
        while (temp[m - 1] > temp[m])
            m--;
        var k = n;
        while (temp[m - 1] > temp[k - 1])
            k--;
        var swapVar =
            //swap m and k
            temp[m - 1];
        temp[m - 1] = temp[k - 1];
        temp[k - 1] = swapVar;

        var p = m + 1;
        var q = n;
        while (p < q)
        {
            swapVar = temp[p - 1];
            temp[p - 1] = temp[q - 1];
            temp[q - 1] = swapVar;
            p++;
            q--;
        }

        return temp;
    }
}

public class PermutationsEnumerable<T> : IEnumerable<IList<T>>
{
    private readonly IList<T> _list;
    private readonly int _r;
    private readonly long _rfact;
    private readonly long _total;

    public PermutationsEnumerable(IList<T> list, int r, long rfact, long total)
    {
        _list = list;
        _r = r;
        _rfact = rfact;
        _total = total;
    }

    public IEnumerator<IList<T>> GetEnumerator()
    {
        return new PermutationsEnumerator<T>(_list, _r, _rfact, _total);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class PermutationsEnumerator<T> : IEnumerator<IList<T>>
{
    private readonly IList<T> _list;
    private readonly int _r;
    private readonly long _rfact;
    private readonly long _total;
    private int[] _currCombination;
    private int[] _currPermutation;
    private int _index = -1;
    private int _permNo;

    public PermutationsEnumerator(IList<T> list, int r, long rfact, long total)
    {
        _currPermutation = new int[r];
        _currCombination = new int[r];
        _r = r;
        _rfact = rfact;
        _total = total;
        _list = list;
    }

    public bool MoveNext()
    {
        _index++;
        return _index < _total;
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public IList<T> Current
    {
        get
        {
            if (_index == 0)
            {
                _permNo = 0;
                for (var i = 0; i < _currCombination.Length; i++)
                {
                    _currCombination[i] = i + 1;
                    _currPermutation[i] = i + 1;
                }
            }
            else if ((_permNo + 1) % _rfact == 0)
            {
                _permNo++;
                _currCombination =
                    CombinationGenerator.GenerateNextCombination(
                        _currCombination, _list.Count, _r);
                for (var i = 0; i < _currCombination.Length; i++)
                    _currPermutation[i] = i + 1;
            }
            else
            {
                _permNo++;
                _currPermutation =
                    PermutationGenerator.GenerateNextPermutation(
                        _currPermutation, _r);
            }

            var result = new List<T>();
            for (var i = 0; i < _r; i++)
                result.Add(
                    _list[_currCombination[_currPermutation[i] - 1] - 1]);
            return result;
        }
    }

    object? IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}