using System;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Evolution.Activation
{
    public class SigmaFunction : EvolvingActivationFunctionBase
    {
        public SigmaFunction(List<double> parameters) : base(parameters)
        {
        }

        public SigmaFunction() : base(new List<double> { 1, 1, 1 })
        {
        }

        public override string Name { get; } = "SigmaFunction";

        public override double Phi(double arg)
        {
            double a = Parameters[0];
            double b = Parameters[1];
            double c = Parameters[2];
            return a / (b + Math.Exp(-arg * c) + 1);
        }

        public override double Dphi(double arg) =>
            throw new NotImplementedException();

        public override object Clone() =>
            new SigmaFunction(Parameters);
    }
}
