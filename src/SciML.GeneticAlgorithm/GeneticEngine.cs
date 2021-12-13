using System;
using System.Collections.Generic;

namespace SciML.GeneticAlgorithm
{
    public class GeneticEngine<C, T> where C : IChromosome<C> where T : IComparable<T>
    {
        internal readonly IFitness<C, T> _fitnessFunction;
        internal readonly ChromosomeComparer<C, T> _chromosomeComparer;

        private readonly int _originalPopulationSize;

        public GeneticEngine(Population<C> population, IFitness<C, T> fitnessFunction)
        {
            Population = population;
            _originalPopulationSize = population.Size;
            _fitnessFunction = fitnessFunction;
            _chromosomeComparer = new ChromosomeComparer<C, T>(this);
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

        public virtual void Evolve()
        {
            /**
             * Removes the worst chromosomes and
             * return population size back to the original size
             */
            Population.SortPopulationByFitness(_chromosomeComparer);
            Population.Trim(_originalPopulationSize);

            /**
             * Generates new population based on existing one
             */
            Population<C> newPopulation = new Population<C>();

            for (int i = 0; i < _originalPopulationSize && i < ParentChromosomesSurviveCount; i++)
            {
                newPopulation.AddChromosome(Population.GetChromosomeByIndex(i));
            }

            for (int i = 0; i < _originalPopulationSize; i++)
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
        /// Gets fitness score of specified chromosome
        /// </summary>
        /// <param name="chromosome">chromosome instance</param>
        /// <returns>fitness score as <see cref="T"/></returns>
        public T Fitness(C chromosome) => _chromosomeComparer.Fit(chromosome);

        /// <summary>
        /// Clears cache of chromosomes comparator
        /// </summary>
        public void ClearCache() => _chromosomeComparer.ClearCache();
    }
}
