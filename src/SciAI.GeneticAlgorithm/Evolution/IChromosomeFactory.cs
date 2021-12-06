namespace SciAI.GeneticAlgorithm.Evolution
{
    public interface IChromosomeFactory
    {
        C GenerateChromosome<C>() where C : IEvolvingChromosome<C>;
    }
}
