// <copyright file="SingleObjectiveSelection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;

namespace Impulse.Shared.GeneticAlgorithm.SelectionOperators
{
    public static class SingleObjectiveSelection
    {
        private static Random random = new Random();

        public static T Tournament<T, T2>(List<T> individuals) where T : IIndividual<T2>
        {
            // Randomly Select two candidartes from the population
            var (candidateA, candidateB) = GetCandidateParents(individuals);

            return candidateA.Fitness[0] < candidateB.Fitness[0]
                ? candidateA
                : candidateB;
        }

        public static (T, T) GetCandidateParents<T>(List<T> population)
        {
            // Grab two random individuals from the population
            var candidateA = population[random.Next(population.Count())];
            var candidateB = population[random.Next(population.Count())];

            // Ensure that the two individuals are unique
            while (candidateA.Equals(candidateB))
            {
                candidateB = population[random.Next(population.Count())];
            }

            return (candidateA, candidateB);
        }
    }
}
