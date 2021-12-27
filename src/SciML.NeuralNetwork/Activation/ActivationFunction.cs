using System;

namespace SciML.NeuralNetwork.Activation
{
    public abstract class ActivationFunction
    {
        public abstract string Name { get; }

        public static ActivationFunction Get<T>(T type) where T : ActivationFunction =>
            Activator.CreateInstance(type.GetType()) as T;

        public abstract double Phi(double arg);

        public abstract double Dphi(double arg);

        //Returns hyperbolic secant of arg
        protected double Sech(double arg) =>
            Math.Abs(arg) < 22d ?
            2d / (Math.Exp(arg) + Math.Exp(-arg)) :
            0d;

    }
}
