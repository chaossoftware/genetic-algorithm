using SciAI.NeuralNetwork.Base;
using System.Collections.Generic;

namespace SciAI.NeuralNetwork.Networks
{
    public abstract class NN2LayerBase<I, O, S> : NeuralNetBase where I : INeuron<I> where O : INeuron<O> where S : Synapse
    {
        protected NN2LayerBase(int inputNeurons, int outputNeurons)
        {
            InputLayer = new Layer<I>(inputNeurons);
            OutputLayer = new Layer<O>(outputNeurons);

            Connections = new List<S>();
        }

        public Layer<I> InputLayer { get; }

        public Layer<O> OutputLayer { get; }

        public List<S> Connections { get; }
    }
}
