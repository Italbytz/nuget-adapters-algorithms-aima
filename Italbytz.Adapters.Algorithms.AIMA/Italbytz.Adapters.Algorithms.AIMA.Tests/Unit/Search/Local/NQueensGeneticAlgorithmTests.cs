using Italbytz.AI.Search.Local;
using Italbytz.AI.Tests.Environment.NQueens;
using Italbytz.AI.Util;
using Italbytz.AI.Util.Datastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Italbytz.AI.Tests.Unit.Search.Local;

public class NQueensGeneticAlgorithmTests
{
    private const bool ConsoleLogging = true;
    private ILoggerFactory _loggerFactory = NullLoggerFactory.Instance;

    [SetUp]
    public void Setup()
    {
        if (ConsoleLogging)
            _loggerFactory =
                LoggerFactory.Create(builder => builder.AddConsole());
    }

    [TearDown]
    public void Cleanup()
    {
        _loggerFactory.Dispose();
    }

    [Test]
    public void TestNQueens()
    {
        var alphabet = new List<int>
        {
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7
        };
        var algo = new GeneticAlgorithm<int>(8, alphabet, 0.3);
        var initPopulation = new List<IIndividual<int>>();
        var random = ThreadSafeRandomNetCore.LocalRandom;
        for (var i = 0; i < 100; i++)
        {
            var randomRepresentation = new List<int>(8);
            for (var j = 0; j < 8; j++)
                randomRepresentation.Add(random.Next(8));
            initPopulation.Add(new Individual<int>(randomRepresentation));
        }

        var result = algo.Execute(initPopulation, FitnessFn, 100);
        var finalFitness = FitnessFn(result);

        Assert.That(finalFitness, Is.EqualTo(1.0));
        return;

        double FitnessFn(IIndividual<int> individual)
        {
            var board = new NQueensBoard(8);
            var x = 0;
            foreach (var y in individual.Representation)
            {
                board.AddQueenAt(new XYLocation(x, y));
                x++;
            }

            return 1.0 / (1.0 + board.GetNumberOfAttackingPairs());
        }
    }
}