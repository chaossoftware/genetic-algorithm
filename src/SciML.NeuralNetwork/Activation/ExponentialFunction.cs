using System;

namespace SciML.NeuralNetwork.Activation;

public sealed class ExponentialFunction : IActivationFunction
{
    public string Name => "Exponential";

    public double Phi(double arg) => Math.Exp(arg);

    public double Dphi(double arg) => Math.Exp(arg);
}
