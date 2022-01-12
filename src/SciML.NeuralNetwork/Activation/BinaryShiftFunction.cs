namespace SciML.NeuralNetwork.Activation
{
    public class BinaryShiftFunction : ActivationFunctionBase
    {
        public override string Name => "Binary shift";

        public override double Phi(double arg) => arg % 1d;

        public override double Dphi(double arg) => 1d;
    }
}
