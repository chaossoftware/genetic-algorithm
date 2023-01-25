using System;

namespace SciML.NeuralNetwork.Activation;

public sealed class GaussianDerivativeFunction : IActivationFunction
{
    public string Name => "Gaussian Derivative";

    public double Phi(double arg) => -arg * Math.Exp(-arg * arg);

    public double Dphi(double arg) => (2d * arg - 1d) * Math.Exp(-arg * arg);
}
