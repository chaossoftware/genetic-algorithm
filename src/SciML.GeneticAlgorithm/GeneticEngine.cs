using System;
using System.Collections.Generic;

namespace SciML.GeneticAlgorithm
{
    /// <summary>
    /// Genetic engine has defined initial population, fitness function and evolution params.<br/>
    /// The engine evolves from initial population using chromosomes mutation and crossover via
    /// selecting best individuals based on fitness function results.
    /// </summary>
    /// <typeparam name="C">cromosome type</typeparam>
    /// <typeparam name="T">fitness measure type</typeparam>
    public class GeneticEngine<C, T> where C : IChromosome<C> where T : IComparable<T>
    {
        internal readonly IFitness<C, T> _fitnessFunction;
        private readonly ChromosomeComparer<C, T> _chromosomeComparer;
        private readonly int _originalPopulationSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneticEngine{C, T}"/> class 
        /// based on initial population and fitness function.
        /// </summary>
        /// <param name="population">initial population for evolution process</param>
        /// <param name="fitnessFunction">function to calculate chromosomes fitness</param>
        public GeneticEngine(Population<C> population, IFitness<C, T> fitnessFunction)
        {
            Population = population;
            _originalPopulationSize = population.Size;
            _fitnessFunction = fitnessFunction;
            _chromosomeComparer = new ChromosomeComparer<C, T>(_fitnessFunction);
            Population.SortPopulationByFitness(_chromosomeComparer);
        }

        /// <summary>
        /// Gets or sets evolving population
        /// </summary>
        public Population<C> Population { get; protected set; }

        /// <summary>
        /// Gets the best chromosome in population (with index 0)
        /// </summary>
        public C BestChromosome => Population.GetChromosomeByIndex(0);

        /// <summary>
        /// Gets the worst chromosome in population (with largest index)
        /// </summary>
        public C WorstChromosome => Population.GetChromosomeByIndex(Population.Size - 1);

        /// <summary>
        /// Gets number of parental chromosomes, which survive (and move to new population)
        /// </summary>
        public int ParentChromosomesSurviveCount { get; set; } = int.MaxValue;

        /// <summary>
        /// Performs population evolution step:<br/> 
        ///  - sorts population by fitness score<br/> 
        ///  - trims population to original size<br/>
        ///  - generates new population based on survived parent chromosomes (without mutation) 
        ///  and mutation and crossover of rest of chromosomes.
        /// </summary>
        public virtual void Evolve()
        {
            // Removes the worst chromosomes and returns population size back to the original size
            Population.SortPopulationByFitness(_chromosomeComparer);

            if (Population.Size > _originalPopulationSize)
            {
                Population.Trim(_originalPopulationSize);
            }

            // Generates new population based on existing one
            Population<C> newPopulation = new Population<C>();

            int survivedParents = Math.Min(Population.Size, ParentChromosomesSurviveCount);

            for (int i = 0; i < survivedParents; i++)
            {
                newPopulation.AddChromosome(Population.GetChromosomeByIndex(i));
            }

            for (int i = 0; i < Population.Size; i++)
            {
                C chromosome = Population.GetChromosomeByIndex(i);
                C mutated = chromosome.Mutate();

                C otherChromosome = Population.GetRandomChromosome();
                List<C> crossovered = chromosome.Crossover(otherChromosome);

                newPopulation.AddChromosome(mutated);

                foreach (C crossoveredChromosome in crossovered)
                {
                    newPopulation.AddChromosome(crossoveredChromosome);
                }
            }

            Population = newPopulation;
        }

        /// <summary>
        /// Simulate extinction event by killing of 90% of population ignoring chromosome fitness.
        /// </summary>
        public void CallMassExtinction()
        {
            int chromosomesToKill = (int)(Population.Size * 0.9);

            for (int i = 0; i < chromosomesToKill; i++)
            {
                Population.RemoveChromosome(Population.GetRandomChromosome());
            }
        }

        /// <summary>
        /// Gets fitness score of specified chromosome.
        /// </summary>
        /// <param name="chromosome">chromosome instance</param>
        /// <returns>fitness score in form of fitness measure type</returns>
        public T GetFitnessOf(C chromosome) => _chromosomeComparer.Fit(chromosome);

        /// <summary>
        /// Clears cache of chromosomes comparator.
        /// </summary>
        public void ClearCache() => _chromosomeComparer.ClearCache();
    }
}
