using System;

namespace Italbytz.AI.Search.Local;

public class Scheduler
{
    private readonly int _k, _limit;
    private readonly double _lam;

    public Scheduler(int k, double lam, int limit)
    {
        _k = k;
        _limit = limit;
        _lam = lam;
    }

    public Scheduler() : this(20, 0.045, 100)
    {
    }

    public double GetTemp(int t)
    {
        if (t < _limit)
            return _k * Math.Exp(-1 * _lam * t);
        return 0.0;
    }
}