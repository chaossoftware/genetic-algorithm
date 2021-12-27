using System;

namespace SciML.NeuralNetwork.Base
{
    /// <summary>
    /// Represents neuron synapse.
    /// </summary>
    public class Synapse : ICloneable
    {
        public Synapse(int sourceIndex, int destinationIndex)
        {
            InIndex = sourceIndex;
            OutIndex = destinationIndex;
            Weight = 0;
            Signal = 0;
        }

        public Synapse(int sourceIndex, int destinationIndex, double weight)
        {
            InIndex = sourceIndex;
            OutIndex = destinationIndex;
            Weight = weight;
            Signal = 0;
        }

        /// <summary>
        /// Gets index of neuron in source layer. 
        /// </summary>
        public int InIndex { get; }

        /// <summary>
        /// Gets index of neuron in target layer. 
        /// </summary>
        public int OutIndex { get; }

        /// <summary>
        /// Gets or sets synapse weight.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets input signal.
        /// </summary>
        public double Signal { get; set; }

        /// <summary>
        /// Makes a copy of the synapse with the same pasrameters.
        /// </summary>
        /// <returns>synapse copy</returns>
        public virtual object Clone()
        {
            Synapse synapseCopy = new Synapse(InIndex, OutIndex)
            {
                Weight = Weight,
                Signal = Signal,
            };

            return synapseCopy;
        }
    }
}
