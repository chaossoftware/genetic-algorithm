using System;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Evolution
{
    public abstract class ThresholdFunction
    {
        private static readonly Random Random = new Random();

        public abstract List<double> DefaultParameters { get; }

        public static ThresholdFunction GetRandomFunction()
        {
            switch (Random.Next(3))
            {
                case 0:
                    return new LinearFunction();
                case 1:
                    return new SignumFunction();
                case 2:
                    return new SigmaFunction();
                default:
                    throw new ArgumentException("option is not specified");
            }
        }

        public static ThresholdFunction GetFunction(string name)
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
                    throw new ArgumentException("option is not specified");
            }
        }

        public abstract double Calculate(double value, List<double> parameters);
    }
}
