// <copyright file="SingleObjectiveGA.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using Impulse.Shared.GeneticAlgorithm.SelectionOperators;

namespace Impulse.Shared.GeneticAlgorithm
{
    public class SingleObjectiveGA<T, T2> : GenericGA<T, T2> where T : IIndividual<T2>
    {
        private double previousBestFitness;
        private int noImprovementCount;

        public SingleObjectiveGA() : base()
        {
            previousBestFitness = double.MaxValue;
            RegisterSelector(SingleObjectiveSelection.Tournament<T, T2>);
            RegisterSortProvider(SingleObjectiveSort);
            RegisterConvergenceCheck(CheckSingleObjectiveConvergence);
        }

        // Sort Operators
        private List<T> SingleObjectiveSort(List<T> population)
        {
            return population.OrderBy(individual => individual.Fitness[0]).ToList();
        }

        private List<T> SingleObjectiveWithViolationSort(List<T> population)
        {
            return population
                .OrderBy(individual => individual.ConstraintViolation)
                .ThenBy(individual => individual.Fitness[0]).ToList();
        }

        // Convergence Operators
        private bool CheckSingleObjectiveConvergence(List<T> arg)
        {
            var currentBestFitness = arg[0].Fitness[0];

            if (currentBestFitness < previousBestFitness)
            {
                previousBestFitness = currentBestFitness;
                noImprovementCount = 0;
            }
            else
            {
                noImprovementCount++;
            }

            return noImprovementCount == MaxNoImprovement || CurrentGeneration == MaxGenerations;
        }
    }
}
