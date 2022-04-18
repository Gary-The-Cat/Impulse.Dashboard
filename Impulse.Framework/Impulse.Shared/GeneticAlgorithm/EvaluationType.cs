// <copyright file="EvaluationType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Impulse.Shared.GeneticAlgorithm;

public enum EvaluationType
{
    /// <summary>
    /// Explicit (default) not set state
    /// </summary>
    NotSet = 0,

    /// <summary>
    /// Single objective provlems only.
    /// </summary>
    SingleObjective_NSGA2 = 1,

    /// <summary>
    /// This NSGA2 implementation supports only 2 objective problems.
    /// </summary>
    MultiObjective_NSGA2 = 2,

    /// <summary>
    /// NSGA3 has improvements in ranking, and convergence for many-objective problems and
    /// should only be considered for problems with 3 or more objectives.
    /// </summary>
    NSGA3 = 3
}
