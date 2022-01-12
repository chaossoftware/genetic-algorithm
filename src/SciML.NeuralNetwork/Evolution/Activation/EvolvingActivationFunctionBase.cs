using MersenneTwister;
using SciML.NeuralNetwork.Activation;
using System;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Evolution.Activation
{
    public abstract class EvolvingActivationFunctionBase : ActivationFunctionBase, ICloneable
    {
        protected EvolvingActivationFunctionBase(List<double> parameters)
        {
            Parameters = new List<double>(parameters);
        }

        public List<double> Parameters { get; }

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

        public abstract object Clone();
    }
}
