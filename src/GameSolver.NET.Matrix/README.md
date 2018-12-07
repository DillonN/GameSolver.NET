# GameSolver.NET.Matrix

This project contains the core logic for solving matrix-based games. Layout:

* Solvers - Directory for solver classes
  * MatrixSolver.cs - Contains an abstract class with logic that would apply to all matrix games. Not strictly necessary right now but useful if N-player games are included in the future
  * TwoPlayerSolver.cs - Contains the class to solve nxm bi-matrix games. Can only find mixed solutions for 2x2 games or pure solutions for nxm games.
  * TwoPlayerZeroSum.cs - Contains the class for zero-sum games. Inherits TwoPlayerSolver. Can find mixed solutions for 2xn games in addition to TwoPlayerSolver solvers.
  
