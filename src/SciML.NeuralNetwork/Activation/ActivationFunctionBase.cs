using System;

namespace SciML.NeuralNetwork.Activation
{
    /// <summary>
    /// Abstraction for activation functions.
    /// </summary>
    public abstract class ActivationFunctionBase
    {
        /// <summary>
        /// Gets activation function name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Creates activation function of specified type.
        /// </summary>
        /// <typeparam name="T">activation function type</typeparam>
        /// <param name="type">type of activation function</param>
        /// <returns>instance of activation function</returns>
        public static ActivationFunctionBase Get<T>(T type) where T : ActivationFunctionBase =>
            Activator.CreateInstance(type.GetType()) as T;

        /// <summary>
        /// Calculates function value for given argument.
        /// </summary>
        /// <param name="arg">function argument</param>
        /// <returns>calculated value</returns>
        public abstract double Phi(double arg);

        /// <summary>
        /// derivative
        /// </summary>
        /// <param name="arg"></param>
        /// <returns>calculated value</returns>
        public abstract double Dphi(double arg);

        /// <summary>
        /// Gets hyperbolic secant of argument.
        /// </summary>
        /// <param name="arg">input argument</param>
        /// <returns>value of hyperbolic secant</returns>
        protected double Sech(double arg) =>
            Math.Abs(arg) < 22d ?
            2d / (Math.Exp(arg) + Math.Exp(-arg)) :
            0d;

    }
}
