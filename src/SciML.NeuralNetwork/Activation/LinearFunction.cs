namespace SciML.NeuralNetwork.Activation;

public sealed class LinearFunction : IActivationFunction
{
    public string Name => "Linear";

    public double Phi(double arg) => arg;

    public double Dphi(double arg) => 2d * arg;
}
