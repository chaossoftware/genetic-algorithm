using System;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Evolution.Activation
{
    public class SignumFunction : EvolvingActivationFunctionBase
    {
        public SignumFunction(List<double> parameters) : base(parameters)
        {
        }

        public SignumFunction() : base(new List<double> { 0 })
        {
        }

        public override string Name { get; } = "SignumFunction";

        public override double Phi(double arg)
        {
            double threshold = Parameters[0];
            return arg > threshold ? 1 : 0;
        }

        public override double Dphi(double arg) =>
            throw new NotImplementedException();

        public override object Clone() =>
            new SignumFunction(Parameters);
    }
}
