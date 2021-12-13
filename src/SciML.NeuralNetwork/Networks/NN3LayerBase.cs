using SciML.NeuralNetwork.Base;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Networks
{
    public abstract class NN3LayerBase<I, H, O, S> : NeuralNetBase where I : INeuron<I> where H : INeuron<H> where O : INeuron<O> where S : Synapse
    {
        protected NN3LayerBase(int inputNeurons, int hiddenNeurons, int outputNeurons)
        {
            InputLayer = new Layer<I>(inputNeurons);
            HiddenLayer = new Layer<H>(hiddenNeurons);
            OutputLayer = new Layer<O>(outputNeurons);

            Connections = new List<S>[] { new List<S>(), new List<S>() };
        }

        public Layer<I> InputLayer { get; }

        public Layer<H> HiddenLayer { get; }

        public Layer<O> OutputLayer { get; }

        public List<S>[] Connections { get; }
    }
}
