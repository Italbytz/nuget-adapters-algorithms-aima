using Italbytz.AI.Learning;
using Italbytz.AI.Learning.Inductive;

namespace Italbytz.AI.Tests.Mock.Learning.Learners;

/// <summary>
///     This class is created just for the testing of cross validation wrapper.
///     It does not implement any kind of listener.
/// </summary>
public class SampleParameterizedLearner : IParameterizedLearner
{
    private bool _alpha = true;
    public int ParameterSize { get; set; }

    public void Train(IDataSet ds)
    {
        // Intentionally do nothing
    }

    public string[] Predict(IDataSet ds)
    {
        throw new NotImplementedException();
    }

    public string Predict(IExample e)
    {
        throw new NotImplementedException();
    }

    public int[] Test(IDataSet ds)
    {
        var res = new int[2];
        if (_alpha)
            res[0] = 100;
        else
            res[0] = 70;

        res[1] = ParameterSize;
        _alpha = !_alpha;
        return res;
    }

    public void Train(int size, IDataSet dataSet)
    {
        ParameterSize = size;
        Train(dataSet);
    }
}