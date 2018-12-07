# GameSolver.NET

GameSolver.NET aims to solve game theory problems in a portable way that can be referenced by any .NET program or assembly.

Compatibility with native applications (e.g. C++) can be accomplished easily by exposing methods to COM interop.

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

## Building and running

The core library is built in .NET Standard 2.0 and can be built with any recent version of Visual Studio 2017.

The Core host targets .NET Core 2.1 and requires at least VS 2017 15.7 with the .NET Core workload installed.

The smartphone host targets Xamarin and requires the Xamarin cross-platform workload be installed in VS. 

No precompiled executables are included since all of the benchmarking was done with hardcoded logic (the executables would only run whatever benchmark was last coded). I can build one and send for the target platform of your choice on request.
