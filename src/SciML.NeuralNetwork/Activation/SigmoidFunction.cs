using System;

namespace SciML.NeuralNetwork.Activation
{
    public class SigmoidFunction : ActivationFunction
    {
        public override string Name => "Sigmoid";

        public override double Phi(double arg)
        {
            if (arg < -44d)
            {
                return 0d;
            }
            else if (arg > 44d)
            {
                return 1d;
            }
            else
            {
                return 1d / (1d + Math.Exp(-arg));
            }
        }

        public override double Dphi(double arg)
        {
            if (Math.Abs(arg) > 44d)
            {
                return 0d;
            }
            else
            {
                double argExp = Math.Exp(arg);
                double _v = (1d + argExp);
                return argExp / (_v * _v);
            }
        }
    }
}
