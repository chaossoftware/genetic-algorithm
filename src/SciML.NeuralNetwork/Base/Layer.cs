using System;

namespace SciML.NeuralNetwork.Base
{
    public class Layer<N> where N : INeuron<N>
    {
        public Layer(int neuronsCount)
        {
            Neurons = new N[neuronsCount];
        }

        public N[] Neurons { get; set; }

        public virtual void Process() =>
            Array.ForEach(Neurons, n => n.Process());
    }
}
