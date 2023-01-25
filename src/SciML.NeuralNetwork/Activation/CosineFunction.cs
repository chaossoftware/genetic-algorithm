using System;

namespace SciML.NeuralNetwork.Activation;

public sealed class CosineFunction : IActivationFunction
{
    public string Name => "Cosine";

    public double Phi(double arg) => Math.Cos(arg);

    public double Dphi(double arg) => Math.Cos(arg);
}
