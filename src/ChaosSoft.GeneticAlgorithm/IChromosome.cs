using System.Collections.Generic;

namespace ChaosSoft.GeneticAlgorithm
{
    /// <summary>
    /// Interface for genetic algorithm chromosomes with basic operations.
    /// </summary>
    /// <typeparam name="C">specific chromosome implementation</typeparam>
    public interface IChromosome<C> where C : IChromosome<C>
    {
        /// <summary>
        /// Crossovers current chromosome with another one.
        /// </summary>
        /// <param name="anotherChromosome">chromosome to crossover with</param>
        /// <returns>list of offsprings</returns>
        List<C> Crossover(C anotherChromosome);

        /// <summary>
        /// Performes mutation on current chromosome.
        /// </summary>
        /// <returns>mutated chromosome</returns>
        C Mutate();
    }
}
