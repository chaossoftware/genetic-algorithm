using System;

namespace SciML.NeuralNetwork.Activation
{
    public class ExponentialFunction : ActivationFunctionBase
    {
        public override string Name => "Exponential";

        public override double Phi(double arg) => Math.Exp(arg);

        public override double Dphi(double arg) => Math.Exp(arg);
    }
}
