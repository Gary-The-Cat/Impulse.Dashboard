// <copyright file="IIndividual.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Impulse.Shared.GeneticAlgorithm;

public interface IIndividual<T>
{
    double[] Fitness { get; }

    double ConstraintViolation { get; }

    int Rank { get; set; }

    double CrowdingDistance { get; set; }

    void SetFitness(double[] fitness);

    T GetGenome();

    IIndividual<T> Clone();
}
