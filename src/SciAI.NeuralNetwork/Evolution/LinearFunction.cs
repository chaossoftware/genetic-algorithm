using System.Collections.Generic;

namespace SciAI.NeuralNetwork.Evolution
{
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
}
