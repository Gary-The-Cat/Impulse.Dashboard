// <copyright file="SequenceMutationOperators.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Impulse.Shared.ExtensionMethods;
using Impulse.Shared.GeneticAlgorithm.Individuals;

namespace Impulse.Shared.GeneticAlgorithm.MutationOperators;

public static class SequenceMutationOperators
{
    private static Random random = new Random();

    public static SequenceIndividual Rotate(SequenceIndividual individual)
    {
        // Grab two unique towns
        var (townA, townB) = GetUniqueTowns(individual.Sequence);

        // Grab a reference to the sequence - just to make code below tidier
        var sequence = individual.GetGenome();

        // Determine which of the indices chosen comes before the other
        int firstIndex = townA < townB ? townA : townB;
        int secondIndex = townA > townB ? townA : townB;

        // Grab the head of the sequence
        var newSequence = sequence.Take(firstIndex).ToList();

        // Grab the centre and rotate it
        var middle = sequence.Skip(firstIndex).Take(secondIndex - firstIndex).Reverse();

        // Grab the end of the sequence
        var end = sequence.Skip(secondIndex).ToList();

        // Add all components of the new sequence together
        newSequence.AddRange(middle);
        newSequence.AddRange(end);

        // Return a new individual with our new sequence
        return new SequenceIndividual(newSequence);
    }

    public static SequenceIndividual Swap(SequenceIndividual individual)
    {
        // Grab a copy of our current sequence
        var sequence = individual.Sequence.ToList();

        // Get the indices of the towns we want to swap
        var (townA, townB) = GetUniqueTowns(individual.Sequence);

        sequence.SwapInPlace(townA, townB);

        return new SequenceIndividual(sequence);
    }

    private static (int, int) GetUniqueTowns(List<int> sequence)
    {
        // Randomly select two towns
        var townA = random.Next(sequence.Count());
        var townB = random.Next(sequence.Count());

        // Ensure that the two towns are not the same
        while (townB == townA)
        {
            townB = random.Next(sequence.Count());
        }

        return (townA, townB);
    }
}
