using System;

namespace SciML.NeuralNetwork.Activation
{
    public class CosineFunction : ActivationFunctionBase
    {
        public override string Name => "Cosine";

        public override double Phi(double arg) => Math.Cos(arg);

        public override double Dphi(double arg) => Math.Cos(arg);
    }
}
