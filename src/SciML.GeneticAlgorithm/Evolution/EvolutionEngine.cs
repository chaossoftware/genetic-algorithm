using System;
using System.Collections.Generic;

namespace SciML.GeneticAlgorithm.Evolution
{
    public class EvolutionEngine<C, T> : GeneticEngine<C, T> where C : IEvolvingChromosome<C> where T : IComparable<T>
    {
        private Random probabilityGenerator;

        /// <summary>
        /// delta = (R - D - C * N) * N
        /// 
        /// delta1 = B1 + (R1 - D1)*N1 - R1*M1*N1
        /// delta2 = B2 + (R2 - D2)*N2 + R1*M1*N1
        /// </summary>
        /// <param name="population"></param>
        /// <param name="fitnessFunc"></param>
        public EvolutionEngine(Population<C> population, IFitness<C, T> fitnessFunc)
            : base(population, fitnessFunc)
        {
            probabilityGenerator = new Random();
        }

        public IChromosomeFactory ChromosomeFactory { get; set; }

        public double CrowdingCoefficient { get; set; }

        public override void Evolve()
        {
            // First, kill chromosomes affected by spontaneous death
            SpontaneousChromosomesDeath();

            var uniqueTypes = new HashSet<Type>();

            var currentPopulationSize = Population.Size;
            var cache = new List<C>();

            for (int i = 0; i < currentPopulationSize; i++)
            {
                var c = Population.GetChromosomeByIndex(i);

                if (uniqueTypes.Add(c.GetType()) && ProbabilityRealized(c.SpontaneousBirthRate))
                {
                    Population.AddChromosome(c.Clone());
                }

                if (ProbabilityRealized(c.ReplicationRate))
                {
                    Population.AddChromosome(c.Clone());
                }
                else if (ProbabilityRealized(c.MutationRate))
                {
                    Population.AddChromosome(c.Mutate());
                    cache.Add(c);
                }
            }

            foreach (var chromosomeToKill in cache)
            {
                Population.RemoveChromosome(chromosomeToKill);
            }

            cache.Clear();
            uniqueTypes.Clear();
        }

        private void SpontaneousChromosomesDeath()
        {
            var currentPopulationSize = Population.Size;
            var cache = new List<C>();

            for (int i = 0; i < currentPopulationSize; i++)
            {
                var c = Population.GetChromosomeByIndex(i);

                if (ProbabilityRealized(c.SpontaneousDeathRate))
                {
                    cache.Add(c);
                }
            }

            foreach (var chromosomeToKill in cache)
            {
                Population.RemoveChromosome(chromosomeToKill);
            }

            cache.Clear();
        }

        private bool ProbabilityRealized(double expectedProbability) =>
            expectedProbability > probabilityGenerator.Next(0, 100) / 100d;
    }
}
