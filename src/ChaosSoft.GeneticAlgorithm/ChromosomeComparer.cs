using System;
using System.Collections.Generic;

namespace ChaosSoft.GeneticAlgorithm
{
    internal class ChromosomeComparer<C, T> : IComparer<C> where C : IChromosome<C> where T : IComparable<T>
    {
        private readonly Dictionary<C, T> _cache = new Dictionary<C, T>();
        private readonly IFitness<C, T> _fitnessFunction;

        public ChromosomeComparer(IFitness<C, T> fitnessFunction)
        {
            _fitnessFunction = fitnessFunction;
        }

        public int Compare(C chromosome, C anotherChromosome)
        {
            T chromosomeFit = Fit(chromosome);
            T anotherChromosomeFit = Fit(anotherChromosome);
            int comparisonResult = anotherChromosomeFit.CompareTo(chromosomeFit);
            return comparisonResult;
        }

        public T Fit(C chromosome)
        {
            bool chromosomeFitCached = _cache.TryGetValue(chromosome, out T chromosomeFit);

            if (!chromosomeFitCached)
            {
                chromosomeFit = _fitnessFunction.Calculate(chromosome);
                _cache.Add(chromosome, chromosomeFit);
            }

            return chromosomeFit;
        }

        public void ClearCache() => _cache.Clear();
    }
}
