namespace SciML.GeneticAlgorithm.Evolution
{
    /// <summary>
    /// Wrapper over <see cref="IChromosome{C}"/> with population model parameters.
    /// </summary>
    /// <typeparam name="C"></typeparam>
    public interface ICreature<C> : IChromosome<C> where C : ICreature<C>
    {
        /// <summary>
        /// Gets or sets probability of creature spontaneous birth event.
        /// </summary>
        double SpontaneousBirthRate { get; set; }

        /// <summary>
        /// Gets or sets probability of creature spontaneous death event.
        /// </summary>
        double SpontaneousDeathRate { get; set; }

        /// <summary>
        /// Gets or sets probability of creature mutation event.
        /// </summary>
        double MutationRate { get; set; }

        /// <summary>
        /// Gets or sets probability of creature replication event.
        /// </summary>
        double ReplicationRate { get; set; }

        /// <summary>
        /// Makes full creature clone.
        /// </summary>
        /// <returns>clone of creature</returns>
        C Clone();
    }
}
