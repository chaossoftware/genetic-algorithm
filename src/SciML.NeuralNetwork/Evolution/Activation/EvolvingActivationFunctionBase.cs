using MersenneTwister;
using SciML.NeuralNetwork.Activation;
using System;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Evolution.Activation
{
    /// <summary>
    /// Describes base for activation function of evolving neurtal network.
    /// </summary>
    public abstract class EvolvingActivationFunctionBase : ActivationFunctionBase, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvolvingActivationFunctionBase"/> class based on 
        /// activation function parameters list.
        /// </summary>
        /// <param name="parameters">function parameters list</param>
        protected EvolvingActivationFunctionBase(List<double> parameters)
        {
            Parameters = new List<double>(parameters);
        }

        /// <summary>
        /// Gets list of activation function parameters.
        /// </summary>
        public List<double> Parameters { get; }

        /// <summary>
        /// Gets random activation function (LinearFunction, SignumFunction, SigmaFunction)
        /// </summary>
        /// <returns>activation function instance</returns>
        public static EvolvingActivationFunctionBase GetRandomFunction()
        {
            const int FunctionsCount = 3;

            switch (Randoms.FastestInt32.Next(FunctionsCount))
            {
                case 0:
                    return new LinearFunction();
                case 1:
                    return new SignumFunction();
                case 2:
                    return new SigmaFunction();
                default:
                    throw new ArgumentException($"Only {FunctionsCount} activation functions exist");
            }
        }

        /// <summary>
        /// Gets activation function by it's name.
        /// </summary>
        /// <param name="name">Activation function name (should be the same as function Type name)</param>
        /// <returns>activation function instance</returns>
        public static EvolvingActivationFunctionBase GetFunction(string name)
        {
            switch (name)
            {
                case "LinearFunction":
                    return new LinearFunction();
                case "SignumFunction":
                    return new SignumFunction();
                case "SigmaFunction":
                    return new SigmaFunction();
                default:
                    throw new ArgumentException($"{name} activation function does not exist");
            }
        }

        /// <summary>
        /// Creates a copy of current activation function.
        /// </summary>
        /// <returns>activation function copy</returns>
        public abstract object Clone();
    }
}
