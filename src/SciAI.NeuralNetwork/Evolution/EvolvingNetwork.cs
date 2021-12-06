using SciAI.NeuralNetwork.Base;
using SciAI.NeuralNetwork.Networks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SciAI.NeuralNetwork.Evolution
{
    public abstract class EvolvingNetwork : ThreeLayerNetwork<TresholdNeuron, TresholdNeuron, TresholdNeuron, Synapse>
    {
        private const double WeightsMutationInterval = 1;
        private const double NeuronParamsMutationInterval = 1;

        protected int activationIterations = 1;

        protected Random random = new Random();

        protected EvolvingNetwork(int inputNeurons, int hiddenNeurons, int outputNeurons) : base(inputNeurons, hiddenNeurons, outputNeurons)
        {
        }

        public int NeuronsCount => this.InputLayer.Neurons.Length + this.HiddenLayer.Neurons.Length + this.OutputLayer.Neurons.Length;

        public List<double> GetWeightsOfSynapses()
        {
            var weights = new List<double>();

            for (int i = 0; i < 2; i++)
            {
                Connections[i].ForEach(s => weights.Add(s.Weight));
            }

            return weights;
        }

        public void SetWeightsOfSynapses(List<double> weights)
        {
            var d1 = Connections[0].Count;
            var d2 = Connections[1].Count;

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
            int inputs = this.InputLayer.Neurons.Length;
            int hidden = this.HiddenLayer.Neurons.Length;

            if (index < inputs)
            {
                return this.InputLayer.Neurons[index];
            }
            else if (index < inputs + this.HiddenLayer.Neurons.Length)
            {
                return this.HiddenLayer.Neurons[index - inputs];
            }
            else
            {
                return this.OutputLayer.Neurons[index - inputs - hidden];
            }
        }

        public void SetNeuron(int index, TresholdNeuron neuron)
        {
            int inputs = this.InputLayer.Neurons.Length;
            int hidden = this.HiddenLayer.Neurons.Length;

            if (index < inputs)
            {
                this.InputLayer.Neurons[index] = neuron;
            }
            else if (index < inputs + this.HiddenLayer.Neurons.Length)
            {
                this.HiddenLayer.Neurons[index - inputs] = neuron;
            }
            else
            {
                this.OutputLayer.Neurons[index - inputs - hidden] = neuron;
            }
        }

        public override void Process()
        {
            this.InputLayer.Process();
            this.HiddenLayer.Process();
            this.OutputLayer.Process();
        }

        public List<T> RandomCrossover<T>(T anotherChromosome) where T : EvolvingNetwork
        {
            T thisClone = this.Clone() as T;
            T anotherClone = anotherChromosome.Clone() as T;

            switch (this.random.Next(3))
            {
                case 0:
                    this.TwoPointsWeightsCrossover(thisClone, anotherClone);
                    break;
                case 1:
                    this.UniformelyDistributedWeightsCrossover(thisClone, anotherClone);
                    break;
                case 2:
                    this.TwoPointsNeuronsCrossover(thisClone, anotherClone);
                    break;
                case 3:
                    this.UniformelyDistributedNeuronsCrossover(thisClone, anotherClone);
                    break;
            }

            var offsprings = new List<T>();

            offsprings.Add(anotherClone);
            offsprings.Add(thisClone);
            offsprings.Add(anotherClone.RandomMutate<T>());
            offsprings.Add(thisClone.RandomMutate<T>());

            return offsprings;
        }

        public T RandomMutate<T>() where T : EvolvingNetwork
        {
            T mutatedBrain = this.Clone() as T;

            switch (this.random.Next(4))
            {
                case 0:
                    this.MutateWeights(mutatedBrain);
                    break;
                case 1:
                    this.MutateNeuronsFunctionsParams(mutatedBrain);
                    break;
                case 2:
                    this.MutateChangeNeuronsFunctions(mutatedBrain);
                    break;
                case 3:
                    this.ShuffleWeightsOnSubinterval(mutatedBrain);
                    break;
            }

            return mutatedBrain;
        }

        #region "Mutation techniques"

        protected void MutateWeights<T>(T mutated) where T : EvolvingNetwork
        {
            var weights = mutated.GetWeightsOfSynapses();

            int currentIndex;
            int weightsSize = weights.Count;
            int iterations = this.random.Next(1, weightsSize);
            var used = new HashSet<int>();

            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    currentIndex = this.random.Next(weightsSize);
                }
                while (used.Contains(currentIndex));

                weights[currentIndex] += (this.Gauss2() - this.Gauss2()) * WeightsMutationInterval;
                used.Add(currentIndex);
            }

            mutated.SetWeightsOfSynapses(weights);
        }

        protected void MutateNeuronsFunctionsParams<T>(T mutated) where T : EvolvingNetwork
        {
            int currentIndex;
            int neuronsSize = mutated.NeuronsCount;
            int iterations = this.random.Next(1, neuronsSize);
            var used = new HashSet<int>();

            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    currentIndex = this.random.Next(neuronsSize);
                }
                while (used.Contains(currentIndex));

                TresholdNeuron n = mutated.GetNeuron(currentIndex);

                List<double> parameters = n.GetTresholdParameters();

                for (int j = 0; j < parameters.Count; j++)
                {
                    parameters[j] += (this.Gauss2() - this.Gauss2()) * NeuronParamsMutationInterval;
                }

                n.SetFunctionAndParams(n.TresholdFunction, parameters);
                used.Add(currentIndex);
            }
        }

        protected void MutateChangeNeuronsFunctions<T>(T mutated) where T : EvolvingNetwork
        {
            int currentIndex;
            int neuronsSize = mutated.NeuronsCount;
            int iterations = this.random.Next(1, neuronsSize);
            var used = new HashSet<int>();

            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    currentIndex = this.random.Next(neuronsSize);
                }
                while (used.Contains(currentIndex));

                TresholdNeuron n = mutated.GetNeuron(currentIndex);

                ThresholdFunction f = ThresholdFunction.GetRandomFunction();
                n.SetFunctionAndParams(f, f.DefaultParameters);
                used.Add(currentIndex);
            }
        }

        protected void ShuffleWeightsOnSubinterval<T>(T mutated) where T : EvolvingNetwork
        {
            List<double> weights = mutated.GetWeightsOfSynapses();

            int left = this.random.Next(weights.Count);
            int right = this.random.Next(weights.Count);

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

            subListOfWeights = subListOfWeights.OrderBy(item => this.random.Next()).ToList(); //shuffle

            for (int i = 0; i < ((right - left) + 1); i++)
            {
                weights[left + i] = subListOfWeights[i];
            }

            mutated.SetWeightsOfSynapses(weights);
        }

        #endregion

        #region "Crossover techniques"

        protected void TwoPointsWeightsCrossover<T>(T thisClone, T anotherClone) where T : EvolvingNetwork
        {
            List<double> thisWeights = thisClone.GetWeightsOfSynapses();
            List<double> anotherWeights = anotherClone.GetWeightsOfSynapses();

            int left = this.random.Next(thisWeights.Count);
            int right = this.random.Next(thisWeights.Count);

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

        protected void UniformelyDistributedWeightsCrossover<T>(T thisClone, T anotherClone) where T : EvolvingNetwork
        {
            List<double> thisWeights = thisClone.GetWeightsOfSynapses();
            List<double> anotherWeights = anotherClone.GetWeightsOfSynapses();

            int currentIndex;
            int weightsSize = thisWeights.Count;
            int iterations = this.random.Next(1, weightsSize);
            var used = new HashSet<int>();

            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    currentIndex = this.random.Next(weightsSize);
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

        protected void TwoPointsNeuronsCrossover<T>(T thisClone, T anotherClone) where T : EvolvingNetwork
        {
            int neuronsSize = thisClone.NeuronsCount;
            int left = this.random.Next(neuronsSize);
            int right = this.random.Next(neuronsSize);

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

        protected void UniformelyDistributedNeuronsCrossover<T>(T thisClone, T anotherClone) where T : EvolvingNetwork
        {
            int currentIndex;
            int neuronsSize = thisClone.NeuronsCount;
            int iterations = this.random.Next(1, neuronsSize);
            var used = new HashSet<int>();

            for (int i = 0; i < iterations; i++)
            {
                do
                {
                    currentIndex = this.random.Next(neuronsSize);
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
                v1 = 2d * random.NextDouble() - 1d;
                v2 = 2d * random.NextDouble() - 1d;
                arg = v1 * v1 + v2 * v2;
            }
            while (arg >= 1d || arg == 0d);

            return v1 * v2 * (-2d + Math.Log(arg) / arg);
        }

        #endregion
    }
}
