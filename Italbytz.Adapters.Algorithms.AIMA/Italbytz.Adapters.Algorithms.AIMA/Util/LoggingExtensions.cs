using Italbytz.AI.Search.Continuous;
using Italbytz.AI.Search.Framework.QSearch;
using Italbytz.AI.Search.Local;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Italbytz.AI.Util;

public static class LoggingExtensions
{
    private static ILogger _geneticAlgorithmLogger = NullLogger.Instance;
    private static ILogger _queueSearchLogger = NullLogger.Instance;
    private static ILogger _hillClimbingSearchLogger = NullLogger.Instance;
    private static ILogger _lpLogger = NullLogger.Instance;

    private static ILogger _simulatedAnnealingSearchLogger =
        NullLogger.Instance;

    public static void InitLoggers(ILoggerFactory loggerFactory)
    {
        _queueSearchLogger = loggerFactory.CreateLogger("QueueSearch");
        _hillClimbingSearchLogger =
            loggerFactory.CreateLogger("HillClimbingSearch");
        _simulatedAnnealingSearchLogger =
            loggerFactory.CreateLogger("SimulatedAnnealingSearch");
        _geneticAlgorithmLogger =
            loggerFactory.CreateLogger("GeneticAlgorithm");
        _lpLogger = loggerFactory.CreateLogger("LP");
    }

    public static void Log<TState, TAction>(
        this QueueSearch<TState, TAction> search, LogLevel logLevel,
        string message)
    {
        _queueSearchLogger.Log(logLevel, message);
    }

    public static void Log(this LPSolver lp, LogLevel logLevel,
        string message)
    {
        _lpLogger.Log(logLevel, message);
    }

    public static void Log<TState, TAction>(
        this HillClimbingSearch<TState, TAction> search, LogLevel logLevel,
        string message)
    {
        _hillClimbingSearchLogger.Log(logLevel, message);
    }

    public static void Log<TState, TAction>(
        this SimulatedAnnealingSearch<TState, TAction> search,
        LogLevel logLevel, string message)
    {
        _simulatedAnnealingSearchLogger.Log(logLevel, message);
    }

    public static void Log<TAlphabet>(
        this GeneticAlgorithm<TAlphabet> search, LogLevel logLevel,
        string message)
    {
        _geneticAlgorithmLogger.Log(logLevel, message);
    }
}