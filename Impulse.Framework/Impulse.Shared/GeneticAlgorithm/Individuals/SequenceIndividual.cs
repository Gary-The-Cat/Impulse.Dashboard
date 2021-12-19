// <copyright file="SequenceIndividual.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Impulse.Shared.GeneticAlgorithm.Individuals
{
    [DebuggerDisplay("{Fitness[0]}")]
    public class SequenceIndividual : IIndividual<List<int>>
    {
        public SequenceIndividual(List<int> genome)
        {
            Sequence = genome;
        }

        public List<int> Sequence { get; set; }

        public double[] Fitness { get; set; }

        public double ConstraintViolation { get; private set; }

        public int Rank { get; set; }

        public double CrowdingDistance { get; set; }

        public IIndividual<List<int>> Clone()
        {
            return new SequenceIndividual(Sequence);
        }

        public override bool Equals(object obj)
        {
            if (obj is SequenceIndividual individual)
            {
                return individual.Sequence.SequenceEqual(this.Sequence);
            }

            return false;
        }

        public List<int> GetGenome()
        {
            return Sequence;
        }

        public void SetFitness(double[] fitness)
        {
            Fitness = fitness;
        }
    }
}
