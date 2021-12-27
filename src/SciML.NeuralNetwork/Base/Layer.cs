using System;

namespace SciML.NeuralNetwork.Base
{
    /// <summary>
    /// Describes layer of neural network.
    /// </summary>
    /// <typeparam name="N">type of layer neurons</typeparam>
    public class Layer<N> where N : INeuron<N>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Layer{N}"/> class with specified 
        /// number of neurons.
        /// </summary>
        /// <param name="neuronsCount">neurons count within the layer</param>
        public Layer(int neuronsCount)
        {
            Neurons = new N[neuronsCount];
        }

        /// <summary>
        /// Gets or sets array of layer neurons.
        /// </summary>
        public N[] Neurons { get; set; }

        /// <summary>
        /// Processes neural net layer (processing of its neurons)
        /// </summary>
        public virtual void Process() =>
            Array.ForEach(Neurons, n => n.Process());
    }
}
