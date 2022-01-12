using SciML.NeuralNetwork.Entities;
using SciML.NeuralNetwork.Evolution.Activation;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Evolution
{
    /// <summary>
    /// Represents neural network neuron with ability to evolve.
    /// </summary>
    public class EvolvingNeuron : INeuron<EvolvingNeuron>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvolvingNeuron"/> class based on specified activation function.
        /// </summary>
        /// <param name="function">neuron activation function</param>
        public EvolvingNeuron(EvolvingActivationFunctionBase function)
        {
            Inputs = new List<Synapse>();
            Outputs = new List<Synapse>();
            ActivationFunction = function;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvolvingNeuron"/> class.
        /// </summary>
        public EvolvingNeuron()
        {
            Inputs = new List<Synapse>();
            Outputs = new List<Synapse>();
        }

        /// <summary>
        /// Gets or sets list of input synapses.
        /// </summary>
        public List<Synapse> Inputs { get; set; }

        /// <summary>
        /// Gets or sets list of output synapses.
        /// </summary>
        public List<Synapse> Outputs { get; set; }

        /// <summary>
        /// Gets or sets neuron activation function.
        /// </summary>
        public EvolvingActivationFunctionBase ActivationFunction { get; set; }

        /// <summary>
        /// Processes input signals through the neuron (processes all inputs via 
        /// activation function and multiplies to weight).
        /// </summary>
        public virtual void Process()
        {
            double arg = 0;

            foreach (Synapse synapse in Inputs)
            {
                arg += synapse.Signal;
            }

            arg = ActivationFunction.Phi(arg);

            foreach (Synapse synapse in Outputs)
            {
                synapse.Signal = synapse.Weight * arg;
            }
        }

        /// <summary>
        /// Makes a copy of the neuron.
        /// </summary>
        /// <returns>neuron copy</returns>
        public virtual object Clone() =>
            new EvolvingNeuron(ActivationFunction.Clone() as EvolvingActivationFunctionBase);
    }
}
