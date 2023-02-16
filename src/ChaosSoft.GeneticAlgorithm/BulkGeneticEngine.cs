using System;

namespace ChaosSoft.GeneticAlgorithm
{
    /// <summary>
    /// Wrapper over <see cref="GeneticEngine{C, T}"/> to perform iterative evolution process
    /// with ability to subscribe to iteration events.
    /// </summary>
    /// <typeparam name="C">cromosome type</typeparam>
    /// <typeparam name="T">fitness measure type</typeparam>
    public class BulkGeneticEngine<C, T> : GeneticEngine<C, T> where C : IChromosome<C> where T : IComparable<T>
    {
        private bool terminate = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="BulkGeneticEngine{C, T}"/> class 
        /// based on initial population and fitness function.
        /// </summary>
        /// <param name="population">initial population for evolution process</param>
        /// <param name="fitnessFunction">function to calculate chromosomes fitness</param>
        public BulkGeneticEngine(Population<C> population, IFitness<C, T> fitnessFunction)
            : base(population, fitnessFunction)
        {

        }

        /// <summary>
        /// Delegate for <see cref="OnIterate"/> event.
        /// </summary>
        /// <param name="engineInstance"></param>
        public delegate void GeneticIterationEvent(BulkGeneticEngine<C, T> engineInstance);

        /// <summary>
        /// Event called after each evolution step execution.
        /// </summary>
        public event GeneticIterationEvent OnIterate;

        /// <summary>
        /// Gets or sets evolution iteration.
        /// </summary>
        public int Iteration { get; protected set; } = 0;

        /// <summary>
        /// Performs specified number of evolution steps (<see cref="GeneticEngine{C, T}.Evolve"/>) 
        /// and calling <see cref="OnIterate"/> after each iteration. 
        /// The operation could be terminated.
        /// </summary>
        /// <param name="iterations">number of iterations to perform</param>
        public void Evolve(int iterations)
        {
            terminate = false;

            for (int i = 0; i < iterations; i++)
            {
                if (terminate)
                {
                    break;
                }

                Evolve();
            }
        }

        /// <summary>
        /// Performs single evolution step (<see cref="GeneticEngine{C, T}.Evolve"/>) 
        /// and calls <see cref="OnIterate"/> event.
        /// </summary>
        public override void Evolve()
        {
            base.Evolve();

            Iteration++;

            OnIterate?.Invoke(this);
        }

        /// <summary>
        /// Terminates evolution process.
        /// </summary>
        public void Terminate() =>
            terminate = true;
    }
}
