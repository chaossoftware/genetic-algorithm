using System;

namespace SciML.NeuralNetwork.Activation
{
    public class HyperbolicTangentFunction : ActivationFunction
    {
        public override string Name => "Hyperbolic tangent";

        public override double Phi(double arg) =>
            arg < 22d ?
            1d - 2d / (Math.Exp(2d * arg) + 1d) :
            Math.Sign(arg);

        public override double Dphi(double arg)
        {
            double tmp = Sech(arg);
            return tmp * tmp;
        }
    }
}
