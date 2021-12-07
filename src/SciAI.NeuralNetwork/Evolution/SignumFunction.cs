using System.Collections.Generic;

namespace SciAI.NeuralNetwork.Evolution
{
    public class SignumFunction : ThresholdFunction
    {
        public override double Calculate(double value, List<double> parameters)
        {
            double threshold = parameters[0];
            return value > threshold ? 1 : 0;
        }

        public override List<double> DefaultParameters => new List<double> { 0 };
    }
}
