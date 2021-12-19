// <copyright file="SequenceSpawner.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Impulse.Shared.ExtensionMethods;
using Impulse.Shared.GeneticAlgorithm.Individuals;

namespace Impulse.Shared.GeneticAlgorithm.Spawners
{
    public static class SequenceSpawner
    {
        public static List<SequenceIndividual> SpawnPopulation(int populationCount, int sequenceLength)
        {
            var population = new List<SequenceIndividual>();

            // Generate {PopulationCount} individuals
            while (population.Count < populationCount)
            {
                var individual = GenerateIndividual(sequenceLength);
                if (!population.Contains(individual))
                {
                    population.Add(individual);
                }
            }

            return population;
        }

        public static SequenceIndividual GenerateIndividual(int sequenceLength)
        {
            // Generate a list of numbers [0, 1, 2, 3... 9]
            var sequence = Enumerable.Range(0, sequenceLength).ToList();

            // Randomly shuffle the list [3, 1, 5, 9... 4]
            sequence.Shuffle();

            // Create a new individual with our random sequence
            return new SequenceIndividual(sequence);
        }
    }
}
