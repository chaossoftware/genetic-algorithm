using System;

namespace SciML.NeuralNetwork.Base
{
    /// <summary>
    /// Base for neural networks implementation (requires network to be cloneable).
    /// </summary>
    public abstract class NeuralNetBase : ICloneable
    {
        /// <summary>
        /// Constructs neural network by specified logic.
        /// </summary>
        public abstract void ConstructNetwork();

        /// <summary>
        /// Clones current neural network.
        /// </summary>
        /// <returns>neural network copy</returns>
        public abstract object Clone();

        /// <summary>
        /// Processes the neural network (processing of all its layers).
        /// </summary>
        public abstract void Process();
    }
}
