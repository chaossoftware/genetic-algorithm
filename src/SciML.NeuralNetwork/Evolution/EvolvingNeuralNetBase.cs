using SciML.NeuralNetwork.Base;
using SciML.NeuralNetwork.Networks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SciML.NeuralNetwork.Evolution
{
    public abstract class EvolvingNeuralNetBase : NN3LayerBase<TresholdNeuron, TresholdNeuron, TresholdNeuron, Synapse>
    {
        private const double WeightsMutationInterval = 1;
        private const double NeuronParamsMutationInterval = 1;

        protected EvolvingNeuralNetBase(int inputNeurons, int hiddenNeurons, int outputNeurons) 
            : base(inputNeurons, hiddenNeurons, outputNeurons)
        {
        }

        public int NeuronsCount => InputLayer.Neurons.Length + HiddenLayer.Neurons.Length + OutputLayer.Neurons.Length;

        protected Random Randomizer { get; } = new Random();

        protected int ActivationIterations { get; set; } = 1;

        public List<double> GetWeightsOfSynapses()
        {
            List<double> weights = new List<double>();

            for (int i = 0; i < 2; i++)
            {
                Connections[i].ForEach(s => weights.Add(s.Weight));
            }

            return weights;
        }

        public void SetWeightsOfSynapses(List<double> weights)
        {
            int d1 = Connections[0].Count;
            int d2 = Connections[1].Count;

            for (int i = 0; i < d1; i++)
            {
                Connections[0][i].Weight = weights[i];
            }

            for (int i = 0; i < d2; i++)
            {
                Connections[1][i].Weight = weights[d1 + i];
            }
        }

        public TresholdNeuron GetNeuron(int index)
        {
            int inputs = InputLayer.Neurons.Length;
            int hidden = HiddenLayer.Neurons.Length;

            if (index < inputs)
            {
                return InputLayer.Neurons[index];
            }
            else if (index < inputs + HiddenLayer.Neurons.Length)
            {
                return HiddenLayer.Neurons[index - inputs];
            }
            else
            {
                return OutputLayer.Neurons[index - inputs - hidden];
            }
        }

        public void SetNeuron(int index, TresholdNeuron neuron)
        {
            int inputs = InputLayer.Neurons.Length;
            int hidden = HiddenLayer.Neurons.Length;

            if (index < inputs)
            {
                InputLayer.Neurons[index] = neuron;
            }
            else if (index < inputs + HiddenLayer.Neurons.Length)
            {
                HiddenLayer.Neurons[index - inputs] = neuron;
            }
            else
            {
                OutputLayer.Neurons[index - inputs - hidden] = neuron;
            }
        }

        public override void Process()
        {
            InputLayer.Process();
            HiddenLayer.Process();
            OutputLayer.Process();
        }

        public List<T> RandomCrossover<T>(T anotherChromosome) where T : EvolvingNeuralNetBase
        {
            T thisClone = Clone() as T;
            T anotherClone = anotherChromosome.Clone() as T;

            switch (Randomizer.Next(3))
            {
                case 0:
                    TwoPointsWeightsCrossover(thisClone, anotherClone);
                    break;
                case 1:
                    UniformelyDistributedWeightsCrossover(thisClone, anotherClone);
                    break;
                case 2:
                    TwoPointsNeuronsCrossover(thisClone, anotherClone);
                    break;
                case 3:
                    UniformelyDistributedNeuronsCrossover(thisClone, anotherClone);
                    break;
            }

            List<T> offsprings = new List<T>();

            offsprings.Add(anotherClone);
            offsprings.Add(thisClone);
            offsprings.Add(anotherClone.RandomMutate<T>());
            offsprings.Add(thisClone.RandomMutate<T>());

            return offsprings;
        }

        public T RandomMutate<T>() where T : EvolvingNeuralNetBase
        {
            T mutatedBrain = Clone() as T;

            switch (Randomizer.Next(4))
            {
                case 0:
                    MutateWeights(mutatedBrain);
                    break;
                case 1:
                    MutateNeuronsFunctionsParams(mutatedBrain);
                    break;
                case 2:
                    MutateChangeNeuronsFunctions(mutatedBrain);
                    break;
                case 3:
                    ShuffleWeightsOnSubinterval(mutatedBrain);
                    break;
            }

            return mutatedBrain;
        }

        #region "Mutation techniques"

        protected void MutateWeights<T>(T mutated) where T : EvolvingNeuralNetBase
        {
            List<double> weights = mutated.GetWeightsOfSynapses();

            int currentIndex;
            int weightsSize = weights.Count;
            int iterations = Randomizer.Next(1, weightsSize);
            HashSet<int> used = new HashSet<int>();

            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    currentIndex = Randomizer.Next(weightsSize);
                }
                while (used.Contains(currentIndex));

                weights[currentIndex] += (Gauss2() - Gauss2()) * WeightsMutationInterval;
                used.Add(currentIndex);
            }

            mutated.SetWeightsOfSynapses(weights);
        }

        protected void MutateNeuronsFunctionsParams<T>(T mutated) where T : EvolvingNeuralNetBase
        {
            int currentIndex;
            int neuronsSize = mutated.NeuronsCount;
            int iterations = Randomizer.Next(1, neuronsSize);
            HashSet<int> used = new HashSet<int>();

            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    currentIndex = Randomizer.Next(neuronsSize);
                }
                while (used.Contains(currentIndex));

                TresholdNeuron n = mutated.GetNeuron(currentIndex);

                List<double> parameters = n.GetTresholdParameters();

                for (int j = 0; j < parameters.Count; j++)
                {
                    parameters[j] += (Gauss2() - Gauss2()) * NeuronParamsMutationInterval;
                }

                n.SetFunctionAndParams(n.TresholdFunction, parameters);
                used.Add(currentIndex);
            }
        }

        protected void MutateChangeNeuronsFunctions<T>(T mutated) where T : EvolvingNeuralNetBase
        {
            int currentIndex;
            int neuronsSize = mutated.NeuronsCount;
            int iterations = Randomizer.Next(1, neuronsSize);
            HashSet<int> used = new HashSet<int>();

            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    currentIndex = Randomizer.Next(neuronsSize);
                }
                while (used.Contains(currentIndex));

                TresholdNeuron n = mutated.GetNeuron(currentIndex);

                ThresholdFunction f = ThresholdFunction.GetRandomFunction();
                n.SetFunctionAndParams(f, f.DefaultParameters);
                used.Add(currentIndex);
            }
        }

        protected void ShuffleWeightsOnSubinterval<T>(T mutated) where T : EvolvingNeuralNetBase
        {
            List<double> weights = mutated.GetWeightsOfSynapses();

            int left = Randomizer.Next(weights.Count);
            int right = Randomizer.Next(weights.Count);

            if (left > right)
            {
                int tmp = right;
                right = left;
                left = tmp;
            }

            List<double> subListOfWeights = new List<double>((right - left) + 1);

            for (int i = 0; i < ((right - left) + 1); i++)
            {
                subListOfWeights.Add(weights[left + i]);
            }

            subListOfWeights = subListOfWeights.OrderBy(item => Randomizer.Next()).ToList(); //shuffle

            for (int i = 0; i < ((right - left) + 1); i++)
            {
                weights[left + i] = subListOfWeights[i];
            }

            mutated.SetWeightsOfSynapses(weights);
        }

        #endregion

        #region "Crossover techniques"

        protected void TwoPointsWeightsCrossover<T>(T thisClone, T anotherClone) where T : EvolvingNeuralNetBase
        {
            List<double> thisWeights = thisClone.GetWeightsOfSynapses();
            List<double> anotherWeights = anotherClone.GetWeightsOfSynapses();

            int left = Randomizer.Next(thisWeights.Count);
            int right = Randomizer.Next(thisWeights.Count);

            if (left > right)
            {
                int tmp = right;
                right = left;
                left = tmp;
            }

            for (int i = left; i < right; i++)
            {
                double thisWeight = anotherWeights[i];
                thisWeights[i] = anotherWeights[i];
                anotherWeights[i] = thisWeight;
            }

            thisClone.SetWeightsOfSynapses(thisWeights);
            anotherClone.SetWeightsOfSynapses(anotherWeights);
        }

        protected void UniformelyDistributedWeightsCrossover<T>(T thisClone, T anotherClone) where T : EvolvingNeuralNetBase
        {
            List<double> thisWeights = thisClone.GetWeightsOfSynapses();
            List<double> anotherWeights = anotherClone.GetWeightsOfSynapses();

            int currentIndex;
            int weightsSize = thisWeights.Count;
            int iterations = Randomizer.Next(1, weightsSize);
            HashSet<int> used = new HashSet<int>();

            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    currentIndex = Randomizer.Next(weightsSize);
                }
                while (used.Contains(currentIndex));

                double thisWeight = thisWeights[currentIndex];
                double anotherWeight = anotherWeights[currentIndex];

                anotherWeights[currentIndex] = thisWeight;
                thisWeights[currentIndex] = anotherWeight;
                used.Add(currentIndex);
            }

            thisClone.SetWeightsOfSynapses(thisWeights);
            anotherClone.SetWeightsOfSynapses(anotherWeights);
        }

        protected void TwoPointsNeuronsCrossover<T>(T thisClone, T anotherClone) where T : EvolvingNeuralNetBase
        {
            int neuronsSize = thisClone.NeuronsCount;
            int left = Randomizer.Next(neuronsSize);
            int right = Randomizer.Next(neuronsSize);

            if (left > right)
            {
                int tmp = right;
                right = left;
                left = tmp;
            }

            for (int i = left; i < right; i++)
            {
                TresholdNeuron thisNeuron = thisClone.GetNeuron(i);
                thisClone.SetNeuron(i, anotherClone.GetNeuron(i));
                anotherClone.SetNeuron(i, thisNeuron);
            }
        }

        protected void UniformelyDistributedNeuronsCrossover<T>(T thisClone, T anotherClone) where T : EvolvingNeuralNetBase
        {
            int currentIndex;
            int neuronsSize = thisClone.NeuronsCount;
            int iterations = Randomizer.Next(1, neuronsSize);
            HashSet<int> used = new HashSet<int>();

            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    currentIndex = Randomizer.Next(neuronsSize);
                }
                while (used.Contains(currentIndex));

                TresholdNeuron thisNeuron = thisClone.GetNeuron(currentIndex);
                TresholdNeuron anotherNeuron = anotherClone.GetNeuron(currentIndex);

                anotherClone.SetNeuron(currentIndex, thisNeuron);
                thisClone.SetNeuron(currentIndex, anotherNeuron);
                used.Add(currentIndex);
            }
        }

        private double Gauss2()
        {
            double v1, v2, arg;

            do
            {
                v1 = 2d * Randomizer.NextDouble() - 1d;
                v2 = 2d * Randomizer.NextDouble() - 1d;
                arg = v1 * v1 + v2 * v2;
            }
            while (arg >= 1d || arg == 0d);

            return v1 * v2 * (-2d + Math.Log(arg) / arg);
        }

        #endregion
    }
}
