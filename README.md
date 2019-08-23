# GameSolver.NET

GameSolver.NET aims to solve game theory problems in a portable way that can be referenced by any .NET program or assembly.

Compatibility with native applications (e.g. C++) can be accomplished easily by exposing methods to COM interop. This still requires that a .NET runtime be available on the platform.

The rest of the read-me is meant as a walkthrough of the source for those unfamiliar with the frameworks used. For a comprehensive analysis and the game theory behind the project, see the **[report writeup](GameSolver.pdf)**.

## Layout

GameSolver.NET contains many projects. They are laid out as follows:

* src - Contains the core logic for GameSolver.NET
  * GameSolver.NET.Common - Basic models that could be used by any other project
  * GameSolver.NET.Extensions - Provides extra methods for existing types not from the library
  * GameSolver.NET.Matrix - Core logic for solving games. Tested and supported are 2xn zero-sum mixed solutions, mxn bi-matrix pure solutions, and 2x2 bi-matrix mixed solutions.
  * GameSolver.NET.Population - Unfinished but working population game solvers. Currently only ESS equilibriums are supported.
  * MathNet.Spatial - Third-party library included here to reduce code size and increase portability.
* Hosts - Benchmarking logic and programs
  * GameSolver.NET.Hosts.Benchmarking - Portable platform-agnostic project that sets up and runs benchmarks
  * GameSolver.NET.Hosts.Core - Benchmark host for desktop and server based platforms. Can run on Windows, Linux, or macOS
  * GS.NET.X - Smartphone host. Currently only implemented in Android but support for iOS and Universal Windows Platform (UWP) would take only minutes to implement. UWP apps run on Windows, Windows Phone, and Xbox One. Note the name is abbreviated due to limitations with the Android build chain (directories must be relatively short)
* Tests - Unit test projects that verify the accuracy of the core library. Uses many examples from ECE1657 notes and problem sets. 

## LINQ API

To understand some of the code, one must understand the LINQ API since it is used extensively. 

LINQ works with deferred execution. Commands can be built with several directives and are all executed at once when the results are enumerated. This will happen if e.g. the command is iterated over in a `foreach` loop, or if certain LINQ methods such as `Min()` or `First()` are called.

LINQ commands work on any type that implements `IEnumerable`. This is the base interface for all collection or list-like types in C#, and simply provides a method to iterate over the elements. GameSolver.NET primarily works with `IReadOnlyList` types to represent matrices, which inherits from `IEnumerable`. 

Examples of deferred execution methods/commands:

* `Select` and `SelectMany` - transform the enumerable based on an expression, e.g. converting strings to ints.
* `Where` - filter the enumerable based on an expression, e.g. where values are above threshold
* `OrderBy()` - sort the enumerable based on an expression. Requires one O(n^2) (worst-case) calculation on its first call. However, in the special case where `First()` is the call, it does the job of `Min()` or `Max()` and is O(n)

Examples of LINQ commands that will iterate the enumerable and execute any built up commands:

* `Any()` - checks if the enumerable contains any items based on a certain expression, only iterates until it has found one
* `Min()` - finds the minimum in the enumerable, iterates the entire expression
* `First()` - works similar to `Any()` except returns the first item instead of a boolean of whether an item exists or not
* `ToList()` and `ToArray()` - convert the enumerable to a list or array, iterates the entire expression

Objects that may appear as an evaulated expression could in fact be a built up command list. E.g. in TwoPlayerZeroSum.cs line 149 a complicated LINQ expression is built, but no evaluation actually occurs until line 166 when `First()` is called.

One last important C# specific concept is `yield return`. This allows the developer to implement their own deferred-execution builders: the method will run until it hits the first `yield return` expression and not execute further until the caller asks for the next element. This is utilized in `TwoPlayerZeroSum.BestResponses()` to create an enumerable of best response functions.

## Building and running

The core library is built in .NET Standard 2.0 and can be built with any recent version of Visual Studio 2017.

The Core host targets .NET Core 2.1 and requires at least VS 2017 15.7 with the .NET Core workload installed.

The smartphone host targets Xamarin and requires the Xamarin cross-platform workload be installed in VS. 

With a compatible version of VS installed, simply open GameSolver.NET.sln to load all of the source.

No precompiled executables are included since all of the benchmarking was done with hardcoded logic (the executables would only run whatever benchmark was last coded). I can build one and send for the target platform of your choice on request.
