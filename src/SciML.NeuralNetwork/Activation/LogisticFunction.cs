namespace SciML.NeuralNetwork.Activation;

public sealed class LogisticFunction : IActivationFunction
{
    public string Name => "Logistic";

    public double Phi(double arg) => arg * (1d - arg);

    public double Dphi(double arg) => 1d - 2d * arg;
}
