using System;

namespace SciML.NeuralNetwork.Activation
{
    public class GaussianDerivativeFunction : ActivationFunctionBase
    {
        public override string Name => "Gaussian Derivative";

        public override double Phi(double arg) => -arg * Math.Exp(-arg * arg);

        public override double Dphi(double arg) => (2d * arg - 1d) * Math.Exp(-arg * arg);
    }
}
