namespace SciML.NeuralNetwork.Activation
{
    public class GaussianFunction : ActivationFunction
    {
        public override string Name => "Gaussian";

        public override double Phi(double arg) => arg * (1d - arg);

        public override double Dphi(double arg) => 1d - 2d * arg;
    }
}
