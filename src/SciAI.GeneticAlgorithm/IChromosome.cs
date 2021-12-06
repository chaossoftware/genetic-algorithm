using System.Collections.Generic;

namespace SciAI.GeneticAlgorithm
{
    public interface IChromosome<C> where C : IChromosome<C>
    {
        List<C> Crossover(C anotherChromosome);

        C Mutate();
    }
}
