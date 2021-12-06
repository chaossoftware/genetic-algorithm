using System;
using System.Collections.Generic;

namespace SciAI.GeneticAlgorithm
{
    internal class ChromosomeComparer<C, T> : IComparer<C> where C : IChromosome<C> where T : IComparable<T>
    {
        private readonly Dictionary<C, T> _cache = new Dictionary<C, T>();
        private readonly GeneticEngine<C, T> _engineInstance;

        public ChromosomeComparer(GeneticEngine<C, T> engine)
        {
            _engineInstance = engine;
        }

        public int Compare(C chromosome, C anotherChromosome)
        {
            T chromosomeFit = Fit(chromosome);
            T anotherChromosomeFit = Fit(anotherChromosome);
            int comparisonResult = chromosomeFit.CompareTo(anotherChromosomeFit);
            return comparisonResult;
        }

        public T Fit(C chromosome)
        {
            bool chromosomeFitCached = _cache.TryGetValue(chromosome, out T chromosomeFit);

            if (!chromosomeFitCached)
            {
                chromosomeFit = _engineInstance._fitnessFunction.Calculate(chromosome);
                _cache.Add(chromosome, chromosomeFit);
            }

            return chromosomeFit;
        }

        public void ClearCache() => _cache.Clear();
    }
}
