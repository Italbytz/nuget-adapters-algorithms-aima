using System;
using System.Collections.Generic;
using System.Linq;
using Italbytz.AI.Search.Framework;
using Italbytz.AI.Util;
using Microsoft.Extensions.Logging;

namespace Italbytz.AI.Search.Local;

public interface IProgressTracker<TAlphabet>
{
    void TrackProgress(int itCount, List<Individual<TAlphabet>> population);
}

public class GeneticAlgorithm<TAlphabet>
{
    public const string MetricPopulationSize = "populationSize";
    public const string MetricIterations = "iterations";
    public const string MetricTime = "timeInMSec";
    protected List<TAlphabet> FiniteAlphabet;

    protected int IndividualLength;
    protected double MutationProbability;

    protected Random Random;

    public GeneticAlgorithm(int individualLength,
        List<TAlphabet> finiteAlphabet, double mutationProbability) : this(
        individualLength, finiteAlphabet, mutationProbability,
        ThreadSafeRandomNetCore.LocalRandom)
    {
    }

    private GeneticAlgorithm(int individualLength,
        List<TAlphabet> finiteAlphabet, double mutationProbability,
        Random random)
    {
        IndividualLength = individualLength;
        FiniteAlphabet = finiteAlphabet;
        MutationProbability = mutationProbability;
        Random = random;
    }

    public IMetrics Metrics { get; } = new Metrics();

    public IIndividual<TAlphabet> Execute(
        IEnumerable<IIndividual<TAlphabet>> initPopulation,
        Func<IIndividual<TAlphabet>, double> fitnessFn, int maxIterations)
    {
        return Execute(initPopulation, fitnessFn, GoalTest, 0L);

        bool GoalTest(IIndividual<TAlphabet> state)
        {
            return Metrics.GetInt(MetricIterations) >= maxIterations;
        }
    }

    private IIndividual<TAlphabet> Execute(
        IEnumerable<IIndividual<TAlphabet>> initPopulation,
        Func<IIndividual<TAlphabet>, double> fitnessFn,
        Func<IIndividual<TAlphabet>, bool> goalTest,
        long maxTimeMilliseconds)
    {
        IIndividual<TAlphabet>? bestIndividual = null;
        var population = new List<IIndividual<TAlphabet>>(initPopulation);
        ValidatePopulation(population);
        UpdateMetrics(population, 0, 0L);
        var startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        var itCount = 0;
        do
        {
            //Console.Write(string.Join(", ",population.Select(fitnessFn).Select(o => o.ToString())));
            population = NextGeneration(population, fitnessFn);
            bestIndividual = RetrieveBestIndividual(population, fitnessFn);
            var elapsedTime =
                DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond -
                startTime;
            UpdateMetrics(population, ++itCount, elapsedTime);
            if (maxTimeMilliseconds > 0L &&
                elapsedTime > maxTimeMilliseconds) break;
        } while (!goalTest(bestIndividual));

        NotifyProgressTrackers(itCount, population);
        return bestIndividual;
    }

    private IIndividual<TAlphabet> RetrieveBestIndividual(
        List<IIndividual<TAlphabet>> population,
        Func<IIndividual<TAlphabet>, double> fitnessFn)
    {
        var max = population.Max(fitnessFn);
        return population.First(individual => fitnessFn(individual) == max);
    }

    private List<IIndividual<TAlphabet>> NextGeneration(
        IReadOnlyList<IIndividual<TAlphabet>> population,
        Func<IIndividual<TAlphabet>, double> fitnessFn)
    {
        var newPopulation =
            new List<IIndividual<TAlphabet>>(population.Count());
        foreach (var individual in population)
        {
            var x = RandomSelection(population, fitnessFn);
            var y = RandomSelection(population, fitnessFn);
            var child = Reproduce(x, y);
            if (Random.NextDouble() <= MutationProbability)
                child = Mutate(child);
            newPopulation.Add(child);
        }

        NotifyProgressTrackers(Metrics.GetInt(MetricIterations),
            population);
        return newPopulation;
    }

    private void NotifyProgressTrackers(int itCount,
        IReadOnlyList<IIndividual<TAlphabet>> generation)
    {
        this.Log(LogLevel.Information, itCount.ToString());
    }

    private IIndividual<TAlphabet> Mutate(IIndividual<TAlphabet> child)
    {
        var mutateOffset = Random.Next(IndividualLength);
        var alphaOffset = Random.Next(FiniteAlphabet.Count);
        var mutatedRepresentation =
            new List<TAlphabet>(child.Representation);
        mutatedRepresentation[mutateOffset] = FiniteAlphabet[alphaOffset];
        return new Individual<TAlphabet>(mutatedRepresentation);
    }

    private IIndividual<TAlphabet> Reproduce(IIndividual<TAlphabet> x,
        IIndividual<TAlphabet> y)
    {
        var c = Random.Next(IndividualLength);
        var childRepresentation = new List<TAlphabet>();
        childRepresentation.AddRange(x.Representation.GetRange(0, c));
        childRepresentation.AddRange(
            y.Representation.GetRange(c, IndividualLength - c));
        return new Individual<TAlphabet>(childRepresentation);
    }

    private IIndividual<TAlphabet> RandomSelection(
        IReadOnlyList<IIndividual<TAlphabet>> population,
        Func<IIndividual<TAlphabet>, double> fitnessFn)
    {
        var selected = population.Last();
        var fValues = population.Select(fitnessFn).ToList();
        fValues = Util.Util.Normalize(fValues);
        var prob = Random.NextDouble();
        var totalSoFar = 0.0;
        for (var i = 0; i < fValues.Count; i++)
        {
            totalSoFar += fValues[i];
            if (!(prob <= totalSoFar)) continue;
            selected = population[i];
            break;
        }

        selected.Descendants += 1;
        return selected;
    }

    private void UpdateMetrics(
        IEnumerable<IIndividual<TAlphabet>> population, int itCount,
        long time)
    {
        Metrics.Set(MetricPopulationSize, population.Count());
        Metrics.Set(MetricIterations, itCount);
        Metrics.Set(MetricTime, time);
    }

    private void ValidatePopulation(
        IReadOnlyCollection<IIndividual<TAlphabet>> population)
    {
        if (population.Count < 1)
            throw new Exception(
                "Must start with at least a population of size 1");
        foreach (var individual in population.Where(individual =>
                     individual.Representation.Count != IndividualLength))
            throw new Exception(
                $"Individual [{individual}] in population is not the required length of {IndividualLength}");
    }
}