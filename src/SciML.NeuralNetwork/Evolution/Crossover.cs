using MersenneTwister;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Evolution
{
    /// <summary>
    /// Represents different crossover techniques for <see cref="EvolvingNetBase"/>
    /// </summary>
    public static class Crossover
    {
        /// <summary>
        /// Performs crossover of two instances of <see cref="EvolvingNetBase"/> using random crossover technique.
        /// </summary>
        /// <typeparam name="T">neural net implementation type</typeparam>
        /// <param name="neuralNet1">neural net instance to crossover</param>
        /// <param name="neuralNet2">other neural net instance to crossover</param>
        public static void RandomCrossover<T>(T neuralNet1, T neuralNet2) where T : EvolvingNetBase
        {
            switch (Randoms.FastestInt32.Next(4))
            {
                case 0:
                    TwoPointsNeuronsCrossover(neuralNet1, neuralNet2);
                    break;
                case 1:
                    UniformelyDistributedNeuronsCrossover(neuralNet1, neuralNet2);
                    break;
                case 2:
                    TwoPointsWeightsCrossover(neuralNet1, neuralNet2);
                    break;
                case 3:
                    UniformelyDistributedWeightsCrossover(neuralNet1, neuralNet2);
                    break;

            }
        }

        /// <summary>
        /// Crossovers weights of synapses in random range.
        /// </summary>
        /// <typeparam name="T">neural net implementation type</typeparam>
        /// <param name="neuralNet1">neural net instance to crossover</param>
        /// <param name="neuralNet2">other neural net instance to crossover</param>
        public static void TwoPointsWeightsCrossover<T>(T neuralNet1, T neuralNet2) where T : EvolvingNetBase
        {
            int synapsesCount = neuralNet1.SynapsesCount;

            int left = Randoms.FastestInt32.Next(synapsesCount);
            int right = Randoms.FastestInt32.Next(synapsesCount);

            if (left > right)
            {
                // swap
                right += left;
                left = right - left;
                right -= left;
            }

            for (int i = left; i < right; i++)
            {
                double nn1SynapseWeight = neuralNet1.GetSynapseWeight(i);
                double nn2SynapseWeight = neuralNet2.GetSynapseWeight(i);

                neuralNet1.SetSynapseWeight(i, nn2SynapseWeight);
                neuralNet2.SetSynapseWeight(i, nn1SynapseWeight);
            }
        }

        /// <summary>
        /// Crossovers weights of random synapses.
        /// </summary>
        /// <typeparam name="T">neural net implementation type</typeparam>
        /// <param name="neuralNet1">neural net instance to crossover</param>
        /// <param name="neuralNet2">other neural net instance to crossover</param>
        public static void UniformelyDistributedWeightsCrossover<T>(T neuralNet1, T neuralNet2) where T : EvolvingNetBase
        {
            int synapsesCount = neuralNet1.SynapsesCount;

            int currentIndex;
            int iterations = Randoms.FastestInt32.Next(1, synapsesCount);
            HashSet<int> used = new HashSet<int>();

            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    currentIndex = Randoms.FastestInt32.Next(synapsesCount);
                }
                while (used.Contains(currentIndex));

                double nn1SynapseWeight = neuralNet1.GetSynapseWeight(currentIndex);
                double nn2SynapseWeight = neuralNet2.GetSynapseWeight(currentIndex);

                neuralNet1.SetSynapseWeight(currentIndex, nn2SynapseWeight);
                neuralNet2.SetSynapseWeight(currentIndex, nn1SynapseWeight);

                used.Add(currentIndex);
            }
        }

        /// <summary>
        /// Crossovers neurons in random range.
        /// </summary>
        /// <typeparam name="T">neural net implementation type</typeparam>
        /// <param name="neuralNet1">neural net instance to crossover</param>
        /// <param name="neuralNet2">other neural net instance to crossover</param>
        public static void TwoPointsNeuronsCrossover<T>(T neuralNet1, T neuralNet2) where T : EvolvingNetBase
        {
            int neuronsCount = neuralNet1.NeuronsCount;
            int left = Randoms.FastestInt32.Next(neuronsCount);
            int right = Randoms.FastestInt32.Next(neuronsCount);

            if (left > right)
            {
                // swap
                right += left;
                left = right - left;
                right -= left;
            }

            for (int i = left; i < right; i++)
            {
                EvolvingNeuron nn1Neuron = neuralNet1.GetNeuron(i);
                EvolvingNeuron nn2Neuron = neuralNet2.GetNeuron(i);

                neuralNet1.SetNeuron(i, nn2Neuron);
                neuralNet2.SetNeuron(i, nn1Neuron);
            }
        }

        /// <summary>
        /// Crossovers random neurons.
        /// </summary>
        /// <typeparam name="T">neural net implementation type</typeparam>
        /// <param name="neuralNet1">neural net instance to crossover</param>
        /// <param name="neuralNet2">other neural net instance to crossover</param>
        public static void UniformelyDistributedNeuronsCrossover<T>(T neuralNet1, T neuralNet2) where T : EvolvingNetBase
        {
            int currentIndex;
            int neuronsCount = neuralNet1.NeuronsCount;
            int iterations = Randoms.FastestInt32.Next(1, neuronsCount);
            HashSet<int> used = new HashSet<int>();

            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    currentIndex = Randoms.FastestInt32.Next(neuronsCount);
                }
                while (used.Contains(currentIndex));

                EvolvingNeuron nn1Neuron = neuralNet1.GetNeuron(currentIndex);
                EvolvingNeuron nn2Neuron = neuralNet2.GetNeuron(currentIndex);

                neuralNet1.SetNeuron(currentIndex, nn2Neuron);
                neuralNet2.SetNeuron(currentIndex, nn1Neuron);

                used.Add(currentIndex);
            }
        }
    }
}
