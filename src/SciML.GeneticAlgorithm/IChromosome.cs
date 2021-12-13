using System.Collections.Generic;

namespace SciML.GeneticAlgorithm
{
    public interface IChromosome<C> where C : IChromosome<C>
    {
        List<C> Crossover(C anotherChromosome);

        C Mutate();
    }
}
