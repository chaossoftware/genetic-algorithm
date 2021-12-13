using System;

namespace SciML.NeuralNetwork.Base
{
    public class Synapse : ICloneable
    {
        public Synapse(int sourceIndex, int destinationIndex)
        {
            InIndex = sourceIndex;
            OutIndex = destinationIndex;
            Weight = 0;
            Signal = 0;
        }

        public Synapse(int sourceIndex, int destinationIndex, double weight)
        {
            InIndex = sourceIndex;
            OutIndex = destinationIndex;
            Weight = weight;
            Signal = 0;
        }

        public int InIndex { get; protected set; }

        public int OutIndex { get; protected set; }

        public double Weight { get; set; }

        public double Signal { get; set; }

        public virtual object Clone()
        {
            var synapseCopy = new Synapse(InIndex, OutIndex)
            {
                Weight = Weight,
                Signal = Signal,
            };

            return synapseCopy;
        }
    }
}
