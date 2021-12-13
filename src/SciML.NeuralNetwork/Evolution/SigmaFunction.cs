using System;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Evolution
{
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
