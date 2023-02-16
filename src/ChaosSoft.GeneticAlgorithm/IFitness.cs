using System;

namespace ChaosSoft.GeneticAlgorithm;

/// <summary>
/// Interface for fitness functions to determine best chromosomes in population.
/// </summary>
/// <typeparam name="C">population chromosomes type</typeparam>
/// <typeparam name="T">fitness measure type</typeparam>
public interface IFitness<C, T> where C : IChromosome<C> where T : IComparable<T>
{
    /// <summary>
    /// Assume that chromosome1 is better than chromosome2 <br/>
    /// fit1 = calculate(chromosome1) <br/>
    /// fit2 = calculate(chromosome2) <br/>
    /// So the following condition must be true <br/>
    /// fit1.compareTo(fit2) > 0 <br/>
    /// </summary>
    /// <param name="chromosome">chromosome instance to calculate fit</param>
    /// <returns></returns>
    T Calculate(C chromosome);
}
