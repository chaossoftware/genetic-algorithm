using System;

namespace SciML.GeneticAlgorithm
{
    public class GeneticBulkEngine<C, T> : GeneticEngine<C, T> where C : IChromosome<C> where T : IComparable<T>
    {
        private bool terminate = false;

        public GeneticBulkEngine(Population<C> population, IFitness<C, T> fitnessFunc)
            : base(population, fitnessFunc)
        {

        }

        public delegate void GeneticIterationEvent(GeneticBulkEngine<C, T> engineInstance);

        public event GeneticIterationEvent OnIterate;

        /// <summary>
        /// Gets or sets evolution iteration
        /// </summary>
        public int Iteration { get; protected set; } = 0;

        public void BulkEvolve(int iterations)
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

        public override void Evolve()
        {
            base.Evolve();

            Iteration++;

            OnIterate?.Invoke(this);
        }

        /// <summary>
        /// Terminate evolution process
        /// </summary>
        public void Terminate() =>
            terminate = true;
    }
}
