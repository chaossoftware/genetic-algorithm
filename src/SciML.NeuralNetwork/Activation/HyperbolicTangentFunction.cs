using System;

namespace SciML.NeuralNetwork.Activation;

public sealed class HyperbolicTangentFunction : IActivationFunction
{
    public string Name => "Hyperbolic tangent";

    public double Phi(double arg) =>
        arg < 22d ?
        1d - 2d / (Math.Exp(2d * arg) + 1d) :
        Math.Sign(arg);

    public double Dphi(double arg)
    {
        double tmp = MathExt.Sech(arg);
        return tmp * tmp;
    }
}
