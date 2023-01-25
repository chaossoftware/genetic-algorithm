namespace SciML.NeuralNetwork.Activation;

public sealed class GaussianFunction : IActivationFunction
{
    public string Name => "Gaussian";

    public double Phi(double arg) => arg * (1d - arg);

    public double Dphi(double arg) => 1d - 2d * arg;
}
