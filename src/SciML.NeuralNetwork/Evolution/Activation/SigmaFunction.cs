using System;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Evolution.Activation
{
    /// <summary>
    /// Describes sigma activation function.
    /// </summary>
    public class SigmaFunction : EvolvingActivationFunctionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SigmaFunction"/> class based on 
        /// activation function parameters list.
        /// </summary>
        /// <param name="parameters">function parameters list (should have count = 3)</param>
        public SigmaFunction(List<double> parameters) : base(parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SigmaFunction"/> class 
        /// with default parameters (1, 1, 1).
        /// </summary>
        public SigmaFunction() : base(new List<double> { 1, 1, 1 })
        {
        }

        /// <summary>
        /// Gets activation function name.
        /// </summary>
        public override string Name { get; } = "SigmaFunction";

        /// <summary>
        /// Gets activation function value for input argument.
        /// </summary>
        /// <param name="arg">input argument</param>
        /// <returns>activation function value</returns>
        public override double Phi(double arg)
        {
            double a = Parameters[0];
            double b = Parameters[1];
            double c = Parameters[2];
            return a / (b + Math.Exp(-arg * c) + 1);
        }

        /// <summary>
        /// Gets activation function derivative for input argument.
        /// </summary>
        /// <param name="arg">input argument</param>
        /// <returns>derivative</returns>
        public override double Dphi(double arg) =>
            throw new NotImplementedException();

        /// <summary>
        /// Creates a copy of current function.
        /// </summary>
        /// <returns>activation function copy</returns>
        public override object Clone() =>
            new SigmaFunction(Parameters);
    }
}
