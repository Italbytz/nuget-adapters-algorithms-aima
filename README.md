# Introduction

[![deprecated](https://img.shields.io/badge/status-DEPRECATED-red?style=flat-square)](DEPRECATED.md) **This package is deprecated and will be removed on June 30, 2026.**

## See Also

This package has been superseded by [`nuget-ai`](https://github.com/RobinNunkesser/work/tree/main/artifacts/packages/dotnet/nuget-ai).

## See Also

This package has been superseded by **[`nuget-foundation`](https://github.com/RobinNunkesser/work/tree/main/artifacts/packages/dotnet/nuget-foundation)**.




The [Hexagonal Architecture](https://web.archive.org/web/20180822100852/http://alistair.cockburn.us/Hexagonal+architecture), also known as the Ports and Adapters pattern, is a design approach that emphasizes separation of concerns by isolating the core application logic from external systems like databases, user interfaces, or APIs. This is achieved through the use of "ports" (interfaces) and "adapters" (implementations), enabling easier testing, maintainability, and flexibility in swapping external dependencies without affecting the core logic.

This repository provides C# adapters for many of the algorithms in [Russell](http://www.cs.berkeley.edu/~russell/) and [Norvig's](http://www.norvig.com/) [Artificial Intelligence - A Modern Approach](http://aima.cs.berkeley.edu/)(AIMA). The NuGet package is called [Italbytz.Adapters.Algorithms.AIMA](https://www.nuget.org/packages/Italbytz.Adapters.Algorithms.AIMA) and offers a [docfx](https://italbytz.github.io/nuget-adapters-algorithms-ai/) page. The corresponding ports are in the NuGet package [Italbytz.Ports.Algorithms.AIMA](https://www.nuget.org/packages/Italbytz.Ports.Algorithms.AIMA) (Source: [nuget-ports-algorithms-ai](https://github.com/Italbytz/nuget-ports-algorithms-ai)).

The main external sources of inspiration are

- [Russell](http://www.cs.berkeley.edu/~russell/) And [Norvig's](http://www.norvig.com/) [Artificial Intelligence - A Modern Approach 3rd Edition](http://aima.cs.berkeley.edu/)(AIMA)
- [FrEAK](https://sourceforge.net/projects/freak427/)

## Index of adapters for AIMA algorithms

Note, that there is an official repository for C# implementations called [aima-csharp](https://github.com/aimacode/aima-csharp) which does not use ports and adapters but may provide algorithms not yet provided here. Also note, that the official Python and Java repositories [aima-python](https://github.com/aimacode/aima-python) and [aima-java](https://github.com/aimacode/aima-java) are far more complete. A lot of the code provided here is based on the official Java code.

Here is a table of algorithms, the figure, name of the code in the book, and the file where they are provided in the code. This chart was made for the third edition of the book. The [aima-pseudocode](https://github.com/aimacode/aima-pseudocode) project describes all the algorithms from the book.

|Fig|Page|Name (in book)|Code|
| -------- |:--------:| :-----| :----- |
|2|34|Environment|[AbstractEnvironment](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Agent/AbstractEnvironment.cs)|
|2.1|35|Agent|[SimpleAgent](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Agent/SimpleAgent.cs)|
|3|66|Problem|[GeneralProblem](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/Framework/Problem/GeneralProblem.cs)|
|3.2|68|Romania|[SimplifiedRoadMapOfPartOfRomania](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA.Tests/Environment/Map/SimplifiedRoadMapOfPartOfRomania.cs)|
|3.7|77|Tree-Search|[TreeSearch](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/Framework/QSearch/TreeSearch.cs)|
|3.7|77|Graph-Search|[GraphSearch](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/Framework/QSearch/GraphSearch.cs)|
|3.10|79|Node|[Node](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/Framework/Node.cs)|
|3.14|84|Uniform-Cost-Search|[UniformCostSearch](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/Uninformed/UniformCostSearch.cs)|
|3|92|Best-First search|[BestFirstSearch](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/Informed/BestFirstSearch.cs)|
|3|93|A\* Search|[AStarSearch](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/Informed/AStarSearch.cs)|
|4.2|122|Hill-Climbing|[HillClimbingSearch](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/Local/HillClimbingSearch.cs)|
|4.5|126|Simulated-Annealing|[SimulatedAnnealingSearch](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/Local/SimulatedAnnealingSearch.cs)|
|4.8|129|Genetic-Algorithm|[GeneticAlgorithm](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/Local/GeneticAlgorithm.cs)
|5.3|166|Minimax-Decision|[MinimaxSearch](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/Adversarial/MinimaxSearch.cs)|
|5.7|170|Alpha-Beta-Search|[AlphaBetaSearch](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/Adversarial/AlphaBetaSearch.cs)|
|6|202|CSP|[CSP](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/CSP/CSP.cs)|
|6.1|204|Map CSP|[MapCSP](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/CSP/Examples/MapCSP.cs)|
|6.5|215|Backtracking-Search|[FlexibleBacktrackingSolver](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/CSP/Solver/FlexibleBacktrackingSolver.cs)|
|6.8|221|Min-Conflicts|[MinConflictsSolver](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/CSP/Solver/MinConflictsSolver.cs)|
|6.11|224|Tree-CSP-Solver|[TreeCspSolver](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Search/CSP/Solver/TreeCspSolver.cs)|
|18.5|702|Decision-Tree-Learning|[DecisionTreeLearner](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Learning/Learners/DecisionTreeLearner.cs)|
|18.8|710|Cross-Validation-Wrapper|[CrossValidation](/Italbytz.Adapters.Algorithms.AIMA/Italbytz.Adapters.Algorithms.AIMA/Learning/Inductive/CrossValidation.cs)|


# Getting Started

[csharp-mstest-ai](https://github.com/RobinNunkesser/csharp-mstest-ai) contains a set of unit tests demonstrating the use of the provided ports and adapters.