using SciML.NeuralNetwork.Entities;
using SciML.NeuralNetwork.Networks;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Evolution
{
    /// <summary>
    /// Describes neural net with ability to evolve (ability to mutate and crossover).<br/>
    /// The net is based on 3-layer network
    /// </summary>
    public abstract class EvolvingNetBase : NeuralNet3LayerBase<EvolvingNeuron, EvolvingNeuron, EvolvingNeuron, Synapse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvolvingNetBase"/> class 
        /// with specified count of neurons in each layer.
        /// </summary>
        /// <param name="inputNeurons">neurons in input layer</param>
        /// <param name="hiddenNeurons">neurons in hidden layer</param>
        /// <param name="outputNeurons">neurons in output layer</param>
        protected EvolvingNetBase(int inputNeurons, int hiddenNeurons, int outputNeurons) 
            : base(inputNeurons, hiddenNeurons, outputNeurons)
        {
        }

        /// <summary>
        /// Gets total neural net neurons count.
        /// </summary>
        public int NeuronsCount => InputLayer.Neurons.Length + HiddenLayer.Neurons.Length + OutputLayer.Neurons.Length;

        /// <summary>
        /// Gets total count of neural net synapses.
        /// </summary>
        public int SynapsesCount => Connections[0].Count + Connections[1].Count;

        /// <summary>
        /// Gets or sets neural net metadata.
        /// </summary>
        public string Metadata { get; set; } = string.Empty;

        /// <summary>
        /// Processes the neural network (processing of all its layers).
        /// </summary>
        public override void Process()
        {
            InputLayer.Process();
            HiddenLayer.Process();
            OutputLayer.Process();
        }

        /// <summary>
        /// Crossovers (randomly) clones of current neural net and retrieved one.
        /// </summary>
        /// <typeparam name="T">neural net implementation type</typeparam>
        /// <param name="anotherChromosome">neural net to crossover with current one</param>
        /// <returns>list of offsprings</returns>
        public virtual List<T> Crossover<T>(T anotherChromosome) where T : EvolvingNetBase
        {
            T thisClone = Clone() as T;
            T anotherClone = anotherChromosome.Clone() as T;

            Evolution.Crossover.RandomCrossover(thisClone, anotherClone);

            List<T> offsprings = new List<T>();

            offsprings.Add(anotherClone);
            offsprings.Add(thisClone);
            //offsprings.Add(anotherClone.Mutate<T>());
            //offsprings.Add(thisClone.Mutate<T>());

            return offsprings;
        }

        /// <summary>
        /// Mutates (randomly) clone of current neural net.
        /// </summary>
        /// <typeparam name="T">neural net implementation type</typeparam>
        /// <returns>mutated instance</returns>
        public virtual T Mutate<T>() where T : EvolvingNetBase
        {
            T mutatedBrain = Clone() as T;
            Mutation.RandomMutation(mutatedBrain);
            return mutatedBrain;
        }

        /// <summary>
        /// Gets weight of synapse by its index 
        /// (suppose all synapses between layers neurons are linearized into one sequence).
        /// </summary>
        /// <param name="index">position in linearized sequence</param>
        /// <returns>synapse weight value</returns>
        public double GetSynapseWeight(int index)
        {
            int l1ConnectionsCount = Connections[0].Count;

            if (index < l1ConnectionsCount)
            {
                return Connections[0][index].Weight;
            }
            else
            {
                return Connections[1][index - l1ConnectionsCount].Weight;
            }
        }

        /// <summary>
        /// Sets weight of synapse by its index 
        /// (suppose all synapses between layers neurons are linearized into one sequence).
        /// </summary>
        /// <param name="index">position in linearized sequence</param>
        /// <param name="weight">weight value to set</param>
        public void SetSynapseWeight(int index, double weight)
        {
            int l1ConnectionsCount = Connections[0].Count;

            if (index < l1ConnectionsCount)
            {
                Connections[0][index].Weight = weight;
            }
            else
            {
                Connections[1][index - l1ConnectionsCount].Weight = weight;
            }
        }

        /// <summary>
        /// Gets neuron from specified position
        /// (suppose all layers neurons are linearized into one sequence [input-hidden-output]).
        /// </summary>
        /// </summary>
        /// <param name="index">position in linearized sequence</param>
        /// <returns>neuron instance</returns>
        public EvolvingNeuron GetNeuron(int index)
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

        /// <summary>
        /// Sets neuron at specified position 
        /// (suppose all layers neurons are linearized into one sequence [input-hidden-output]).
        /// </summary>
        /// <param name="index">position in linearized sequence</param>
        /// <param name="neuron">neuron to set</param>
        public void SetNeuron(int index, EvolvingNeuron neuron)
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
    }
}
