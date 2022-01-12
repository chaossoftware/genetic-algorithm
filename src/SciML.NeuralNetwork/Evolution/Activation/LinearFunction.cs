using System.Collections.Generic;

namespace SciML.NeuralNetwork.Evolution.Activation
{
    public class LinearFunction : EvolvingActivationFunctionBase
    {
        public LinearFunction(List<double> parameters) : base(parameters)
        {
        }

        public LinearFunction() : base(new List<double> { 1, 0 })
        {
        }

        public override string Name { get; } = "LinearFunction";

        public override double Phi(double arg)
        {
            double a = Parameters[0];
            double b = Parameters[1];
            return (a * arg) + b;
        }

        public override double Dphi(double arg) =>
            throw new System.NotImplementedException();

        public override object Clone() =>
            new LinearFunction(Parameters);
    }
}
