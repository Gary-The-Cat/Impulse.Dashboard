// <copyright file="RoutePlanner.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Impulse.Shared.GeneticAlgorithm;
using Impulse.Shared.GeneticAlgorithm.CrossoverOperators;
using Impulse.Shared.GeneticAlgorithm.Individuals;
using Impulse.Shared.GeneticAlgorithm.MutationOperators;
using Impulse.Shared.GeneticAlgorithm.Spawners;

namespace Impulse.Dashboard.Debug.DemoScreens.RoutePlannerDemo
{
    public class RoutePlanner
    {
        private string startAddress;
        private string endAddress;
        private List<string> addresses;
        private Func<(string, string), double> getCost;

        public RoutePlanner(
            string startAddress,
            string endAddress,
            List<string> addresses,
            Func<(string, string), double> getCost)
        {
            this.startAddress = startAddress;
            this.endAddress = endAddress;
            this.addresses = addresses;
            this.getCost = getCost;
        }

        public Action<double> ReportProgress { get; set; }

        private int AddressCount => addresses.Count();

        public async Task Solve()
        {
            await Task.Run(() =>
            {
                var solver = ConfigureGeneticAlgorithm();

                solver.SubscribeToFittestIndividualUpdates(value => ReportProgress?.Invoke(value.Fitness[0]));

                solver.StartBreeding();
            });
        }

        private GenericGA<SequenceIndividual, List<int>> ConfigureGeneticAlgorithm()
        {
            var geneticAlgorithm = new SingleObjectiveGA<SequenceIndividual, List<int>>();

            geneticAlgorithm.RegisterSpawner(() => SequenceSpawner.GenerateIndividual(AddressCount));
            geneticAlgorithm.RegisterCrossoverOperator(SequenceCrossoverOperators.DoCrossover);
            geneticAlgorithm.RegisterMutationOperator(SequenceMutationOperators.Rotate, 0.5);
            geneticAlgorithm.RegisterMutationOperator(SequenceMutationOperators.Swap, 0.5);
            geneticAlgorithm.RegisterFitnessCalculator(GetIndividualFitness);
            geneticAlgorithm.Initialise();

            return geneticAlgorithm;
        }

        private double[] GetIndividualFitness(SequenceIndividual individual)
        {
            var decodedSequence = individual.Sequence.Select(s => addresses[s]).ToArray();

            // Calculate the distance from our starting point, to the first address
            var fitness = getCost((startAddress, decodedSequence[0]));

            for (int i = 0; i < decodedSequence.Count() - 1; i++)
            {
                var key = (decodedSequence[i], decodedSequence[i + 1]);

                fitness += getCost(key);
            }

            // Add the distance for our time to the last destination
            fitness += getCost((decodedSequence.Last(), endAddress));

            return new[] { fitness };
        }
    }
}
