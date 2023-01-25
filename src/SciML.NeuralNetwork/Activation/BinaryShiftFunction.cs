namespace SciML.NeuralNetwork.Activation;

public sealed class BinaryShiftFunction : IActivationFunction
{
    public string Name => "Binary shift";

    public double Phi(double arg) => arg % 1d;

    public double Dphi(double arg) => 1d;
}
