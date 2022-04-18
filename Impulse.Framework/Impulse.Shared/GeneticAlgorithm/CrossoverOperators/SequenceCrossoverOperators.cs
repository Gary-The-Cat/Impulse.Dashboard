// <copyright file="SequenceCrossoverOperators.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Linq;
using Impulse.Shared.GeneticAlgorithm.Individuals;

namespace Impulse.Shared.GeneticAlgorithm.CrossoverOperators;

public static class SequenceCrossoverOperators
{
    private static Random random = new Random();

    public static SequenceIndividual DoCrossover(SequenceIndividual individualA, SequenceIndividual individualB)
    {
        // Generate a number between 1 and sequence length - 1 to be our crossover position
        var crossoverPosition = random.Next(1, individualA.Sequence.Count - 1);

        // Grab the head from the first individual
        var offspringSequence = individualA.Sequence.Take(crossoverPosition).ToList();

        // Create a hash for quicker 'exists in head' checks
        var appeared = offspringSequence.ToHashSet();

        // Append individualB to the head, skipping any values that have already shown up in the head
        foreach (var town in individualB.Sequence)
        {
            if (appeared.Contains(town))
            {
                continue;
            }

            offspringSequence.Add(town);
        }

        // Return our new offspring!
        return new SequenceIndividual(offspringSequence);
    }
}
