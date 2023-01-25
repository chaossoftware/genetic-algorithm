using System;

namespace SciML.NeuralNetwork.Activation;

public sealed class PiecewiseLinearFunction : IActivationFunction
{
    public string Name => "Piecewise Linear";

    public double Phi(double arg) =>
        Math.Abs(arg) < 1d ? arg : Math.Sign(arg);

    public double Dphi(double arg) =>
        Math.Abs(arg) < 1d ? 1 : 0;
}
