using SciML.NeuralNetwork.Base;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Networks
{
    /// <summary>
    /// Base for 2-layered neural networks (requires network to be cloneable).
    /// </summary>
    /// <typeparam name="I">type of input neurons</typeparam>
    /// <typeparam name="O">type of output neurons</typeparam>
    /// <typeparam name="S">type of synapses</typeparam>
    public abstract class NN2LayerBase<I, O, S> : NeuralNetBase where I : INeuron<I> where O : INeuron<O> where S : Synapse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NN2LayerBase{I, O, S}"/> class with 
        /// specified number of neurons.
        /// </summary>
        /// <param name="inputNeurons">count of neurons in input layer</param>
        /// <param name="outputNeurons">count of neurons in output layer</param>
        protected NN2LayerBase(int inputNeurons, int outputNeurons)
        {
            InputLayer = new Layer<I>(inputNeurons);
            OutputLayer = new Layer<O>(outputNeurons);

            Connections = new List<S>();
        }

        /// <summary>
        /// Gets network input layer.
        /// </summary>
        public Layer<I> InputLayer { get; }

        /// <summary>
        /// Gets network output layer.
        /// </summary>
        public Layer<O> OutputLayer { get; }

        /// <summary>
        /// Gets all network synapses.
        /// </summary>
        public List<S> Connections { get; }
    }
}
