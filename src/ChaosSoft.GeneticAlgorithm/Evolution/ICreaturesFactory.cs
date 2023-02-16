namespace ChaosSoft.GeneticAlgorithm.Evolution
{
    /// <summary>
    /// Interface for creatures factories.
    /// </summary>
    public interface ICreaturesFactory
    {
        /// <summary>
        /// Constructs new instance of <see cref="ICreature{C}"/>
        /// </summary>
        /// <typeparam name="C">specific creature implementation</typeparam>
        /// <returns>creature instance</returns>
        C GetCreature<C>() where C : ICreature<C>;
    }
}
