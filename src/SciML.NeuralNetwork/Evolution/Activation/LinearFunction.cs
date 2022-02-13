using System.Collections.Generic;

namespace SciML.NeuralNetwork.Evolution.Activation
{
    /// <summary>
    /// Describes linear activation function.
    /// </summary>
    public class LinearFunction : EvolvingActivationFunctionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearFunction"/> class based on 
        /// activation function parameters list.
        /// </summary>
        /// <param name="parameters">function parameters list (should have count = 2)</param>
        public LinearFunction(List<double> parameters) : base(parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearFunction"/> class 
        /// with default parameters (0, 1).
        /// </summary>
        public LinearFunction() : base(new List<double> { 1, 0 })
        {
        }

        /// <summary>
        /// Gets activation function name.
        /// </summary>
        public override string Name { get; } = "LinearFunction";

        /// <summary>
        /// Gets activation function value for input argument.
        /// </summary>
        /// <param name="arg">input argument</param>
        /// <returns>activation function value</returns>
        public override double Phi(double arg)
        {
            double a = Parameters[0];
            double b = Parameters[1];
            return (a * arg) + b;
        }

        /// <summary>
        /// Gets activation function derivative for input argument.
        /// </summary>
        /// <param name="arg">input argument</param>
        /// <returns>derivative</returns>
        public override double Dphi(double arg) =>
            throw new System.NotImplementedException();

        /// <summary>
        /// Creates a copy of current function.
        /// </summary>
        /// <returns>activation function copy</returns>
        public override object Clone() =>
            new LinearFunction(Parameters);
    }
}
