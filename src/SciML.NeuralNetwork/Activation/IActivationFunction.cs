namespace SciML.NeuralNetwork.Activation;

/// <summary>
/// Abstraction for activation functions.
/// </summary>
public interface IActivationFunction
{
    /// <summary>
    /// Gets activation function name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Calculates function value for given argument.
    /// </summary>
    /// <param name="arg">function argument</param>
    /// <returns>calculated value</returns>
    double Phi(double arg);

    /// <summary>
    /// derivative
    /// </summary>
    /// <param name="arg"></param>
    /// <returns>calculated value</returns>
    double Dphi(double arg);
}
