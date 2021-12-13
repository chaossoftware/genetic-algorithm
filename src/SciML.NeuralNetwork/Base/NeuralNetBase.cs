using System;

namespace SciML.NeuralNetwork.Base
{
    public abstract class NeuralNetBase : ICloneable
    {
        public abstract void ConstructNetwork();

        public abstract object Clone();

        public abstract void Process();
    }
}
