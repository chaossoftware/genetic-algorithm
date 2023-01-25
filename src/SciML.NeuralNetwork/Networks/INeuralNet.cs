namespace SciML.NeuralNetwork.Networks;

/// <summary>
/// Base for neural networks implementation (requires network to be cloneable).
/// </summary>
public interface INeuralNet
{
    /// <summary>
    /// Constructs neural network by specified logic.
    /// </summary>
    void ConstructNetwork();

    /// <summary>
    /// Processes the neural network (processing of all its layers).
    /// </summary>
    void Process();
}
