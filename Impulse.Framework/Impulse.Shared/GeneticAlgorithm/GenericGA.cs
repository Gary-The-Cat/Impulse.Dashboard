// <copyright file="GenericGA.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using Impulse.Shared.Datastructures;

namespace Impulse.Shared.GeneticAlgorithm;

public class GenericGA<T, U> where T : IIndividual<U>
{
    protected List<T> population;
    protected SingleCoverageValue<Func<T>> spawnerProvider;
    protected SingleCoverageValue<Func<T, T, T>> crossoverOperatorProvider;
    protected SingleCoverageValue<Func<List<T>, T>> selectionProvider;
    protected SingleCoverageValue<Func<T, T>> mutationOperatorProvider;

    private readonly List<(double, Func<T>)> spawners;
    private readonly List<(double, Func<T, T, T>)> crossoverOperators;
    private readonly List<(double, Func<T, T>)> mutationOperators;
    private readonly List<(double, Func<List<T>, T>)> selectors;
    private Func<List<T>, List<T>> populationSortProvider;
    private Func<T, double[]> fitnessCalculator;
    private Func<List<T>, bool> convergenceCheck;

    private bool isInitialized;

    private Random random;

    public GenericGA()
    {
        population = new List<T>();
        spawners = new List<(double, Func<T>)>();
        crossoverOperators = new List<(double, Func<T, T, T>)>();
        mutationOperators = new List<(double, Func<T, T>)>();
        selectors = new List<(double, Func<List<T>, T>)>();

        random = new Random();
    }

    // [Args] - The fittest individual in the population at the end of a generation
    private event EventHandler<T> GenerationCompletedEvent;

    public int PopulationCount { get; set; } = 300;

    public int MaxNoImprovement { get; set; } = 200;

    public int MaxGenerations { get; set; } = 100000;

    public int CurrentGeneration { get; private set; }

    public double MutationChance { get; set; } = 0.02;

    public bool MutateParentsAsChildren { get; set; } = true;

    public bool MutationEnabled { get; set; } = true;

    public bool CrossoverEnabled { get; set; } = true;

    public void Initialise()
    {
        // Spawner Single Coverage
        var cumulativeValue = 0.0;
        var tempSpawners = new List<DoubleRange<Func<T>>>();
        foreach (var spawner in spawners)
        {
            tempSpawners.Add(new DoubleRange<Func<T>>(
                cumulativeValue,
                cumulativeValue + spawner.Item1,
                spawner.Item2));

            cumulativeValue += spawner.Item1;
        }

        spawnerProvider = new SingleCoverageValue<Func<T>>(tempSpawners);

        // Crossover Single Coverage
        cumulativeValue = 0;
        var tempCrossoverOperators = new List<DoubleRange<Func<T, T, T>>>();
        foreach (var crossoverOperator in crossoverOperators)
        {
            tempCrossoverOperators.Add(new DoubleRange<Func<T, T, T>>(
                cumulativeValue,
                cumulativeValue + crossoverOperator.Item1,
                crossoverOperator.Item2));

            cumulativeValue += crossoverOperator.Item1;
        }

        crossoverOperatorProvider = new SingleCoverageValue<Func<T, T, T>>(tempCrossoverOperators);

        // Mutation Single Coverage
        cumulativeValue = 0;
        var tempMutators = new List<DoubleRange<Func<T, T>>>();
        foreach (var mutationOperator in mutationOperators)
        {
            tempMutators.Add(new DoubleRange<Func<T, T>>(
                cumulativeValue,
                cumulativeValue + mutationOperator.Item1,
                mutationOperator.Item2));

            cumulativeValue += mutationOperator.Item1;
        }

        mutationOperatorProvider = new SingleCoverageValue<Func<T, T>>(tempMutators);

        // Selectors Single Coverage
        cumulativeValue = 0;
        var tempSelectors = new List<DoubleRange<Func<List<T>, T>>>();
        foreach (var selectors in selectors)
        {
            tempSelectors.Add(new DoubleRange<Func<List<T>, T>>(
                cumulativeValue,
                cumulativeValue + selectors.Item1,
                selectors.Item2));

            cumulativeValue += selectors.Item1;
        }

        selectionProvider = new SingleCoverageValue<Func<List<T>, T>>(tempSelectors);

        isInitialized = true;
    }

    public virtual void StartBreeding()
    {
        EnsureValidState();

        // Spawn the initial population
        population = SpawnPopulation();

        // Calculate the fitness of all individuals in the initial population
        UpdateFitness(population);

        while (!HasConverged())
        {
            // Breed parents, approxiamtely doubling the size of the population
            DoGeneration();

            SortPopulation();

            // Take the top 'PopulationCount' worth of individuals
            population = population.Take(PopulationCount).ToList();

            GenerationCompletedEvent?.Invoke(null, population.First());

            CurrentGeneration++;
        }
    }

    public void SubscribeToFittestIndividualUpdates(Action<T> callback)
    {
        GenerationCompletedEvent += (_, i) => callback(i);
    }

    public void UnsubscribeToFittestIndividualUpdates(Action<T> callback)
    {
        GenerationCompletedEvent -= (_, i) => callback(i);
    }

    public void RegisterSpawner(Func<T> spawner, double likelihood = 1.0)
    {
        spawners.Add((likelihood, spawner));
    }

    public void RegisterSelector(Func<List<T>, T> selector, double likelihood = 1.0)
    {
        selectors.Add((likelihood, selector));
    }

    public void RegisterCrossoverOperator(Func<T, T, T> crossOperator, double likelihood = 1.0)
    {
        crossoverOperators.Add((likelihood, crossOperator));
    }

    public void RegisterMutationOperator(Func<T, T> mutationOperator, double likelihood = 1.0)
    {
        mutationOperators.Add((likelihood, mutationOperator));
    }

    public void RegisterFitnessCalculator(Func<T, double[]> fitnessCalculator)
    {
        this.fitnessCalculator = fitnessCalculator;
    }

    protected void RegisterSortProvider(Func<List<T>, List<T>> populationSortProvider)
    {
        this.populationSortProvider = populationSortProvider;
    }

    protected void RegisterConvergenceCheck(Func<List<T>, bool> convergenceCheck)
    {
        this.convergenceCheck = convergenceCheck;
    }

    internal virtual void UpdateFitness(List<T> individuals)
    {
        foreach (var individual in individuals)
        {
            individual.SetFitness(fitnessCalculator(individual));
        }
    }

    internal virtual void SortPopulation()
    {
        population = populationSortProvider(population);
    }

    internal virtual bool HasConverged()
    {
        return convergenceCheck(population);
    }

    internal List<T> SpawnPopulation()
    {
        var population = new List<T>();

        // Create the population
        for (int i = 0; i < PopulationCount; i++)
        {
            population.Add(spawnerProvider.Sample());
        }

        return population;
    }

    internal virtual void DoGeneration()
    {
        // Get individuals
        var newIndividuals = GetOffspring();

        UpdateFitness(newIndividuals);

        foreach (var individual in newIndividuals)
        {
            if (IsUnique(individual, population))
            {
                population.Add(individual);
            }
        }
    }

    internal virtual List<T> GetOffspring()
    {
        var offspring = new List<T>();

        while (offspring.Count() < PopulationCount)
        {
            var mother = selectionProvider.Sample(population);
            var father = selectionProvider.Sample(population);
            T offspringA = default;
            T offspringB = default;

            if (CrossoverEnabled)
            {
                offspringA = crossoverOperatorProvider.Sample(mother, father);
                offspringB = crossoverOperatorProvider.Sample(father, mother);
            }

            // Mutate Offspring A
            if (MutationEnabled && MutationChance >= random.NextDouble())
            {
                offspringA = mutationOperatorProvider.Sample(offspringA);
            }

            // Mutate Offspring B
            if (MutationEnabled && MutationChance >= random.NextDouble())
            {
                offspringB = mutationOperatorProvider.Sample(offspringB);
            }

            if (MutateParentsAsChildren)
            {
                offspring.Add(mutationOperatorProvider.Sample(mother));
                offspring.Add(mutationOperatorProvider.Sample(father));
            }

            offspring.Add(offspringA);
            offspring.Add(offspringB);
        }

        return offspring;
    }

    private void EnsureValidState()
    {
        if (!isInitialized)
        {
            throw new Exception("Ensure Initialise() is called before breeding.");
        }

        if (spawnerProvider == null)
        {
            throw new Exception("Ensure you have provided at least 1 spawner prior to calling Initialise().");
        }

        if (crossoverOperatorProvider == null && CrossoverEnabled)
        {
            throw new Exception("Ensure you have provided at least 1 crossover operator prior to calling Initialise().");
        }

        if (mutationOperatorProvider == null && MutationEnabled)
        {
            throw new Exception("Ensure you have provided at least 1 mutation operator prior to calling Initialise().");
        }
    }

    private bool IsUnique(T individualA, List<T> population)
    {
        foreach (var individualB in population)
        {
            if (individualA.Equals(individualB))
            {
                return false;
            }
        }

        return true;
    }
}
