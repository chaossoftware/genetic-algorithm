using MersenneTwister;
using System;
using System.Collections.Generic;

namespace ChaosSoft.GeneticAlgorithm.Evolution
{
    /// <summary>
    /// Simulation of a population evolution/growth considering population model.<br/>
    /// B - creature spontaneous birth rate<br/>
    /// R - creature spontaneous replication rate<br/>
    /// M - creature spontaneous mutation rate<br/>
    /// D - creature spontaneous death rate<br/>
    /// C - "crowding" coefficient<br/>
    /// N - current number of creatures<br/>
    /// ------------------------------------<br/>
    /// delta = B + (R * (1 - M) - D - C * N) * N<br/>  
    /// => R * (1 - M) : adjusted replication chance per creature<br/>
    /// => - D - C * N : death chance per creature adjusted for crowding<br/>
    /// ------------------------------------<br/>
    /// if there are 2 creature types then:<br/>
    /// delta1 = B1 + (R1 - D1)*N1 - R1*M1*N1<br/>
    /// delta2 = B2 + (R2 - D2)*N2 + R1*M1*N1
    /// </summary>
    /// <typeparam name="C">creature type</typeparam>
    /// <typeparam name="T">fitness measure type</typeparam>
    public class EvolutionEngine<C, T> : GeneticEngine<C, T> where C : ICreature<C> where T : IComparable<T>
    {
        private readonly Random _probabilityGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvolutionEngine{C, T}"/> class 
        /// based on initial population and fitness function.
        /// </summary>
        /// <param name="population"></param>
        /// <param name="fitnessFunc"></param>
        public EvolutionEngine(Population<C> population, IFitness<C, T> fitnessFunc)
            : base(population, fitnessFunc)
        {
            _probabilityGenerator = Randoms.FastestDouble;
        }

        /// <summary>
        /// Gets of sets creatures factory.
        /// </summary>
        public ICreaturesFactory CreaturesFactory { get; set; }

        /// <summary>
        /// Gets or sets crowding coefficient.
        /// </summary>
        public double CrowdingCoefficient { get; set; }

        /// <summary>
        /// Performs single population evolution step. 
        /// Based on fact of probabilities realization kills, replicates, 
        /// mutates or creates new creatures.
        /// </summary>
        public override void Evolve()
        {
            // First, kill creatures affected by spontaneous death
            SpontaneousChromosomesDeath();

            HashSet<Type> uniqueTypes = new HashSet<Type>();

            int currentPopulationSize = Population.Size;
            List<C> cache = new List<C>();

            for (int i = 0; i < currentPopulationSize; i++)
            {
                C c = Population.GetChromosomeByIndex(i);

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

            foreach (C chromosomeToKill in cache)
            {
                Population.RemoveChromosome(chromosomeToKill);
            }

            cache.Clear();
            uniqueTypes.Clear();
        }

        private void SpontaneousChromosomesDeath()
        {
            int currentPopulationSize = Population.Size;
            List<C> cache = new List<C>();

            for (int i = 0; i < currentPopulationSize; i++)
            {
                C c = Population.GetChromosomeByIndex(i);

                if (ProbabilityRealized(c.SpontaneousDeathRate))
                {
                    cache.Add(c);
                }
            }

            foreach (C chromosomeToKill in cache)
            {
                Population.RemoveChromosome(chromosomeToKill);
            }

            cache.Clear();
        }

        private bool ProbabilityRealized(double expectedProbability) =>
            expectedProbability > _probabilityGenerator.NextDouble();
    }
}
