using System;

namespace SciML.NeuralNetwork.Activation
{
    public class PiecewiseLinearFunction : ActivationFunctionBase
    {
        public override string Name => "Piecewise Linear";

        public override double Phi(double arg) =>
            Math.Abs(arg) < 1d ? arg : Math.Sign(arg);

        public override double Dphi(double arg) =>
            Math.Abs(arg) < 1d ? 1 : 0;
    }
}
