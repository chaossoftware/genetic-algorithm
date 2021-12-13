using System;

namespace SciML.NeuralNetwork.Base
{
    public interface INeuron<N> : ICloneable where N : INeuron<N>
    {
        void Process();
    }
}
