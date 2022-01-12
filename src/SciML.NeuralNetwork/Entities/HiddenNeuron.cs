using SciML.NeuralNetwork.Activation;
using System.Collections.Generic;
using System.Linq;

namespace SciML.NeuralNetwork.Entities
{
    /// <summary>
    /// Describes base neuron of hidden layer with activation function.
    /// </summary>
    public class HiddenNeuron : INeuron<HiddenNeuron>
    {
        private readonly ActivationFunctionBase _activationFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="HiddenNeuron"/> class 
        /// with specified activation function.
        /// </summary>
        /// <param name="activationFunction">activation function instance</param>
        public HiddenNeuron(ActivationFunctionBase activationFunction)
        {
            Inputs = new List<Synapse>();
            Outputs = new List<Synapse>();
            _activationFunction = activationFunction;
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
        /// Makes a copy of the neuron with the same activation function.
        /// </summary>
        /// <returns>neuron copy</returns>
        public virtual object Clone() =>
            new HiddenNeuron(_activationFunction);

        /// <summary>
        /// Processes input signals through the neuron (processes all inputs via 
        /// activation function and multiplies to weight).
        /// </summary>
        public virtual void Process()
        {
            double arg = Inputs.Sum(s => s.Signal);
            Outputs.ForEach(s => s.Signal = s.Weight * _activationFunction.Phi(arg));
        }
    }
}
