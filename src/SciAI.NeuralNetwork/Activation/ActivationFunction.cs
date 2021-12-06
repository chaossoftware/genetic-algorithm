using System;

namespace SciAI.NeuralNetwork.Activation
{
    public abstract class ActivationFunction
    {
        public abstract string Name { get; }

        public static ActivationFunction Get(ActivationFunctionType activationFunction)
        {
            switch (activationFunction)
            {
                case ActivationFunctionType.BinaryShift:
                    return new BinaryShiftFunction();
                case ActivationFunctionType.Cosine:
                    return new CosineFunction();
                case ActivationFunctionType.Linear:
                    return new LinearFunction();
                case ActivationFunctionType.Logistic:
                    return new LogisticFunction();
                case ActivationFunctionType.Exponential:
                    return new ExponentialFunction();
                case ActivationFunctionType.Gaussian:
                    return new GaussianFunction();
                case ActivationFunctionType.GaussianDerivative:
                    return new GaussianDerivativeFunction();
                case ActivationFunctionType.HyperbolicTangent:
                    return new HyperbolicTangentFunction();
                case ActivationFunctionType.PiecewiseLinear:
                    return new PiecewiseLinearFunction();
                case ActivationFunctionType.Sigmoid:
                    return new SigmoidFunction();
                default:
                    throw new NotImplementedException($"'{activationFunction}' activatino function is not implemented yet");
            }
        }

        public abstract double Phi(double arg);

        public abstract double Dphi(double arg);

        //Returns hyperbolic secant of arg
        protected double Sech(double arg) =>
            Math.Abs(arg) < 22d ?
            2d / (Math.Exp(arg) + Math.Exp(-arg)) :
            0d;

    }
}
