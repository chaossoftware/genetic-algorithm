using SciML.NeuralNetwork.Entities;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Networks
{
    /// <summary>
    /// Base for 3-layered neural networks (requires network to be cloneable).
    /// </summary>
    /// <typeparam name="I">type of input neurons</typeparam>
    /// <typeparam name="H">type of hidden neurons</typeparam>
    /// <typeparam name="O">type of output neurons</typeparam>
    /// <typeparam name="S">type of synapses</typeparam>
    public abstract class NeuralNet3LayerBase<I, H, O, S> : NeuralNetBase 
        where I : INeuron<I> where H : INeuron<H> where O : INeuron<O> where S : Synapse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NeuralNet3LayerBase{I, H, O, S}"/> class with 
        /// specified number of neurons.
        /// </summary>
        /// <param name="inputNeurons">count of neurons in input layer</param>
        /// <param name="hiddenNeurons">count of neurons in hidden layer</param>
        /// <param name="outputNeurons">count of neurons in output layer</param>
        protected NeuralNet3LayerBase(int inputNeurons, int hiddenNeurons, int outputNeurons)
        {
            InputLayer = new Layer<I>(inputNeurons);
            HiddenLayer = new Layer<H>(hiddenNeurons);
            OutputLayer = new Layer<O>(outputNeurons);

            Connections = new List<S>[] { new List<S>(), new List<S>() };
        }

        /// <summary>
        /// Gets network input layer.
        /// </summary>
        public Layer<I> InputLayer { get; }

        /// <summary>
        /// Gets network hidden layer.
        /// </summary>
        public Layer<H> HiddenLayer { get; }

        /// <summary>
        /// Gets network output layer.
        /// </summary>
        public Layer<O> OutputLayer { get; }

        /// <summary>
        /// Gets all network synapses.
        /// </summary>
        public List<S>[] Connections { get; }
    }
}
