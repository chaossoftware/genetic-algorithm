namespace SciML.NeuralNetwork.Activation
{
    public class LogisticFunction : ActivationFunctionBase
    {
        public override string Name => "Logistic";

        public override double Phi(double arg) => arg * (1d - arg);

        public override double Dphi(double arg) => 1d - 2d * arg;
    }
}
