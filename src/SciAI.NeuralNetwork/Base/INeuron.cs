using System;

namespace SciAI.NeuralNetwork.Base
{
    public interface INeuron<N> : ICloneable where N : INeuron<N>
    {
        void Process();
    }
}
