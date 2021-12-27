using System.Collections.Generic;

namespace SciML.NeuralNetwork.Base
{
    /// <summary>
    /// Describes base neuron of input layer.
    /// </summary>
    public class InputNeuron : INeuron<InputNeuron>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputNeuron"/> class.
        /// </summary>
        public InputNeuron()
        {
            Outputs = new List<Synapse>();
        }

        /// <summary>
        /// Gets or sets input synapse.
        /// </summary>
        public Synapse Input { get; set; }

        /// <summary>
        /// Gets or sets list of output synapses.
        /// </summary>
        public List<Synapse> Outputs { get; set; }

        /// <summary>
        /// Makes a copy of the neuron.
        /// </summary>
        /// <returns>neuron copy</returns>
        public virtual object Clone() =>
            new InputNeuron();

        /// <summary>
        /// Processes signal through the neuron (multiplies signal by weight).
        /// </summary>
        public virtual void Process() =>
            Outputs.ForEach(s => s.Signal = Input.Signal * s.Weight);
    }
}
