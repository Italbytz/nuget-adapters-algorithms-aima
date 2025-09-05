# Introduction

The [Hexagonal Architecture](https://web.archive.org/web/20180822100852/http://alistair.cockburn.us/Hexagonal+architecture), also known as the Ports and Adapters pattern, is a design approach that emphasizes separation of concerns by isolating the core application logic from external systems like databases, user interfaces, or APIs. This is achieved through the use of "ports" (interfaces) and "adapters" (implementations), enabling easier testing, maintainability, and flexibility in swapping external dependencies without affecting the core logic.

This repository provides C# adapters for Artificial Intelligence projects. The NuGet package is called [Italbytz.Adapters.Algorithms.AI](https://www.nuget.org/packages/Italbytz.Adapters.Algorithms.AI) and offers a [docfx](https://italbytz.github.io/nuget-adapters-algorithms-ai/) page. The corresponding ports are in the NuGet package [Italbytz.Ports.Algorithms.AI](https://www.nuget.org/packages/Italbytz.Ports.Algorithms.AI) (Source: [nuget-ports-algorithms-ai](https://github.com/Italbytz/nuget-ports-algorithms-ai)).

This repository only contains the shared code of these subprojects:

- [Italbytz.Adapters.Algorithms.ML](https://github.com/Italbytz/nuget-adapters-algorithms-ml) for extensions to Microsoft's [ML.NET](https://dot.net/ml)
- [Italbytz.Adapters.Algorithms.AIMA](https://github.com/Italbytz/nuget-adapters-algorithms-aima) for many of the algorithms in [Russell](http://www.cs.berkeley.edu/~russell/) and [Norvig's](http://www.norvig.com/) [Artificial Intelligence - A Modern Approach](http://aima.cs.berkeley.edu/)(AIMA)
- [Italbytz.Adapters.Algorithms.EA](https://github.com/Italbytz/nuget-adapters-algorithms-ea) for Evolutionary Algorithms inspired by [FrEAK](https://sourceforge.net/projects/freak427/)