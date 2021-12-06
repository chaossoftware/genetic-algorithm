namespace SciAI.GeneticAlgorithm.Evolution
{
    public interface IEvolvingChromosome<C> : IChromosome<C> where C : IEvolvingChromosome<C>
    {
        double SpontaneousBirthRate { get; set; }

        double SpontaneousDeathRate { get; set; }

        double MutationRate { get; set; }

        double ReplicationRate { get; set; }

        C Clone();
    }
}
