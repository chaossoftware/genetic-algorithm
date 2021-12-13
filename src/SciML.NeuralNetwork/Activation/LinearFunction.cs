namespace SciML.NeuralNetwork.Activation
{
    public class LinearFunction : ActivationFunction
    {
        public override string Name => "Linear";

        public override double Phi(double arg) => arg;

        public override double Dphi(double arg) => 2d * arg;
    }
}
