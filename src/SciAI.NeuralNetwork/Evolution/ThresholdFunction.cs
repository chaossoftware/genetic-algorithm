using System;
using System.Collections.Generic;

namespace SciAI.NeuralNetwork.Evolution
{
    public abstract class ThresholdFunction
    {
        private static readonly Random random = new Random();

        public abstract List<double> DefaultParameters { get; }

        public static ThresholdFunction GetRandomFunction()
        {
            switch (random.Next(3))
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

    public class LinearFunction : ThresholdFunction
    {
        public override double Calculate(double value, List<double> parameters)
        {
            double a = parameters[0];
            double b = parameters[1];
            return (a * value) + b;
        }

        public override List<double> DefaultParameters => new List<double> { 1, 0 };
    }

    public class SignumFunction : ThresholdFunction
    {
        public override double Calculate(double value, List<double> parameters)
        {
            double threshold = parameters[0];
            return value > threshold ? 1 : 0;
        }

        public override List<double> DefaultParameters => new List<double> { 0 };
    }

    public class SigmaFunction : ThresholdFunction
    {
        public override double Calculate(double value, List<double> parameters)
        {
            double a = parameters[0];
            double b = parameters[1];
            double c = parameters[2];
            return a / (b + Math.Exp(-value * c) + 1);
        }

        public override List<double> DefaultParameters => new List<double> { 1, 1, 1 };
    }
}
