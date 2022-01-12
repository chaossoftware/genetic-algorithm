using MersenneTwister;
using SciML.NeuralNetwork.Evolution.Activation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SciML.NeuralNetwork.Evolution
{
    /// <summary>
    /// Represents different mutation techniques for <see cref="EvolvingNetBase"/>
    /// </summary>
    public static class Mutation
    {
        private const double WeightsMutationInterval = 1;
        private const double NeuronParamsMutationInterval = 1;

        /// <summary>
        /// Mutates instance of <see cref="EvolvingNetBase"/> using random mutation technique.
        /// </summary>
        /// <typeparam name="T">neural net implementation type</typeparam>
        /// <param name="neuralNet">neural net instance to mutate</param>
        public static void RandomMutation<T>(T neuralNet) where T : EvolvingNetBase
        {
            switch (Randoms.FastestInt32.Next(4))
            {
                case 0:
                    MutateNeuronsFunctionsParams(neuralNet);
                    break;
                case 1:
                    MutateChangeNeuronsFunctions(neuralNet);
                    break;
                case 2:
                    MutateWeights(neuralNet);
                    break;
                case 3:
                    ShuffleWeightsOnSubinterval(neuralNet);
                    break;
            }
        }

        /// <summary>
        /// Mutates weights of random synapses using random values.
        /// </summary>
        /// <typeparam name="T">neural net implementation type</typeparam>
        /// <param name="neuralNet">neural net instance to mutate</param>
        public static void MutateWeights<T>(T neuralNet) where T : EvolvingNetBase
        {
            int synapsesCount = neuralNet.SynapsesCount;

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

                double synapseWeight = neuralNet.GetSynapseWeight(currentIndex) + (Gauss2() - Gauss2()) * WeightsMutationInterval;

                neuralNet.SetSynapseWeight(currentIndex, synapseWeight);
                used.Add(currentIndex);
            }
        }

        /// <summary>
        /// Mutates params of activation function for random neurons using random values.
        /// </summary>
        /// <typeparam name="T">neural net implementation type</typeparam>
        /// <param name="neuralNet">neural net instance to mutate</param>
        public static void MutateNeuronsFunctionsParams<T>(T neuralNet) where T : EvolvingNetBase
        {
            int currentIndex;
            int neuronsCount = neuralNet.NeuronsCount;
            int iterations = Randoms.FastestInt32.Next(1, neuronsCount);
            HashSet<int> used = new HashSet<int>();

            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    currentIndex = Randoms.FastestInt32.Next(neuronsCount);
                }
                while (used.Contains(currentIndex));

                EvolvingNeuron n = neuralNet.GetNeuron(currentIndex);

                for (int j = 0; j < n.ActivationFunction.Parameters.Count; j++)
                {
                    n.ActivationFunction.Parameters[j] += (Gauss2() - Gauss2()) * NeuronParamsMutationInterval;
                }

                used.Add(currentIndex);
            }
        }

        /// <summary>
        /// Mutates (changes) activation function for random neurons using random values.
        /// </summary>
        /// <typeparam name="T">neural net implementation type</typeparam>
        /// <param name="neuralNet">neural net instance to mutate</param>
        public static void MutateChangeNeuronsFunctions<T>(T neuralNet) where T : EvolvingNetBase
        {
            int currentIndex;
            int neuronsCount = neuralNet.NeuronsCount;
            int iterations = Randoms.FastestInt32.Next(1, neuronsCount);
            HashSet<int> used = new HashSet<int>();

            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    currentIndex = Randoms.FastestInt32.Next(neuronsCount);
                }
                while (used.Contains(currentIndex));

                EvolvingNeuron n = neuralNet.GetNeuron(currentIndex);

                EvolvingActivationFunctionBase f = EvolvingActivationFunctionBase.GetRandomFunction();
                n.ActivationFunction = f;
                used.Add(currentIndex);
            }
        }

        /// <summary>
        /// Mutates (shuffles) weights of synapses in random interval.
        /// </summary>
        /// <typeparam name="T">neural net implementation type</typeparam>
        /// <param name="neuralNet">neural net instance to mutate</param>
        public static void ShuffleWeightsOnSubinterval<T>(T neuralNet) where T : EvolvingNetBase
        {
            int synapsesCount = neuralNet.SynapsesCount;

            int left = Randoms.FastestInt32.Next(synapsesCount);
            int right = Randoms.FastestInt32.Next(synapsesCount);

            if (left > right)
            {
                int tmp = right;
                right = left;
                left = tmp;
            }

            int iterations = (right - left) + 1;

            double[] subListOfWeights = new double[iterations];

            for (int i = 0; i < iterations; i++)
            {
                subListOfWeights[i] = neuralNet.GetSynapseWeight(left + i);
            }

            // shuffle
            subListOfWeights = subListOfWeights
                .OrderBy(item => Randoms.FastestInt32.Next())
                .ToArray();

            for (int i = 0; i < iterations; i++)
            {
                neuralNet.SetSynapseWeight(left + i, subListOfWeights[i]);
            }
        }

        private static double Gauss2()
        {
            double v1, v2, arg;

            do
            {
                v1 = 2d * Randoms.FastestDouble.NextDouble() - 1d;
                v2 = 2d * Randoms.FastestDouble.NextDouble() - 1d;
                arg = v1 * v1 + v2 * v2;
            }
            while (arg >= 1d || arg == 0d);

            return v1 * v2 * (-2d + Math.Log(arg) / arg);
        }
    }
}
