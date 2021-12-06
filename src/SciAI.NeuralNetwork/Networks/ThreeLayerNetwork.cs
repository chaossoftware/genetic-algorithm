using SciAI.NeuralNetwork.Base;
using System.Collections.Generic;

namespace SciAI.NeuralNetwork.Networks
{
    public abstract class ThreeLayerNetwork<I, H, O, S> : NeuralNetBase where I : INeuron<I> where H : INeuron<H> where O : INeuron<O> where S : Synapse
    {
        protected ThreeLayerNetwork(int inputNeurons, int hiddenNeurons, int outputNeurons)
        {
            InputLayer = new Layer<I>(inputNeurons);
            HiddenLayer = new Layer<H>(hiddenNeurons);
            OutputLayer = new Layer<O>(outputNeurons);

            Connections = new List<S>[] { new List<S>(), new List<S>() };
        }

        public Layer<I> InputLayer { get; set; }

        public Layer<H> HiddenLayer { get; set; }

        public Layer<O> OutputLayer { get; set; }

        public List<S>[] Connections { get; set; }
    }
}
