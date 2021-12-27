using MersenneTwister;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SciML.GeneticAlgorithm
{
    /// <summary>
    /// Represents a population of chromosomes
    /// </summary>
    /// <typeparam name="C">cromosome type</typeparam>
    public class Population<C> : IEnumerable<C> where C : IChromosome<C>
    {
        private const int DefaultChromosomesCount = 32;

        private readonly Random _random = Randoms.FastestInt32;

        private List<C> chromosomes = new List<C>(DefaultChromosomesCount);

        /// <summary>
        /// Gets value indicating population size
        /// </summary>
        public int Size => chromosomes.Count;

        /// <summary>
        /// Adds new chromosome to existing ones
        /// </summary>
        /// <param name="chromosome">chromosome instance</param>
        public void AddChromosome(C chromosome) =>
            chromosomes.Add(chromosome);

        /// <summary>
        /// Gets chromosime with random index based on <see cref="Random"/>
        /// </summary>
        /// <returns>chromosome instance</returns>
        public C GetRandomChromosome() =>
            chromosomes[_random.Next(Size)];

        /// <summary>
        /// Gets chromosome with specified index
        /// </summary>
        /// <param name="index">chromosome index in list or chromosomes</param>
        /// <returns>chromosome instance</returns>
        public C GetChromosomeByIndex(int index) =>
            chromosomes[index];

        /// <summary>
        /// Removes specified chromosome from chromosomos list
        /// </summary>
        /// <param name="chromosome">chromosome instance</param>
        public void RemoveChromosome(C chromosome) =>
            chromosomes.Remove(chromosome);

        /// <summary>
        /// Sorts list of existing chromosomes based on fitness score descending
        /// </summary>
        /// <param name="chromosomesComparator">chromosome custom comparator</param>
        public void SortPopulationByFitness(IComparer<C> chromosomesComparator) =>
            chromosomes.Sort(chromosomesComparator);

        /// <summary>
        /// Shortens the population till specific length
        /// </summary>
        /// <param name="length">number of chromosomes to leave in the population</param>
        public void Trim(int length) =>
            chromosomes = chromosomes.GetRange(0, length);

        /// <summary>
        /// Filters population by specified chromosome type
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <returns>list of chromosomes of specified type</returns>
        public List<C> FilterChromosomes<U>() => // where U : IChromosome<U> =>
            chromosomes.Where(c => c is U).ToList();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An System.Collections.IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() =>
            chromosomes.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the chromosomes collection.
        /// </summary>
        /// <returns>Returns an enumerator that iterates through the chromosomes collection</returns>
        public IEnumerator<C> GetEnumerator() =>
            chromosomes.GetEnumerator();
    }

}
