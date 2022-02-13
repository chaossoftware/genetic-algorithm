using System;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Evolution.Activation
{
    public class SignumFunction : EvolvingActivationFunctionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignumFunction"/> class based on 
        /// activation function parameters list.
        /// </summary>
        /// <param name="parameters">function parameters list (should have count = 1)</param>
        public SignumFunction(List<double> parameters) : base(parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignumFunction"/> class 
        /// with default parameters (0).
        /// </summary>
        public SignumFunction() : base(new List<double> { 0 })
        {
        }

        /// <summary>
        /// Gets activation function name.
        /// </summary>
        public override string Name { get; } = "SignumFunction";

        /// <summary>
        /// Gets activation function value for input argument.
        /// </summary>
        /// <param name="arg">input argument</param>
        /// <returns>activation function value</returns>
        public override double Phi(double arg)
        {
            double threshold = Parameters[0];
            return arg > threshold ? 1 : 0;
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
            new SignumFunction(Parameters);
    }
}
