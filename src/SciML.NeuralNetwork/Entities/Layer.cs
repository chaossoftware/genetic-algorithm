namespace SciML.NeuralNetwork.Entities;

/// <summary>
/// Describes layer of neural network.
/// </summary>
/// <typeparam name="N">type of layer neurons</typeparam>
public sealed class Layer<N> where N : INeuron<N>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Layer{N}"/> class with specified 
    /// number of neurons.
    /// </summary>
    /// <param name="neuronsCount">neurons count within the layer</param>
    public Layer(int neuronsCount)
    {
        Neurons = new N[neuronsCount];
    }

    /// <summary>
    /// Gets or sets array of layer neurons.
    /// </summary>
    public N[] Neurons { get; }

    /// <summary>
    /// Processes neural net layer (processing of its neurons)
    /// </summary>
    public void Process()
    {
        for (int i = 0; i < Neurons.Length; i++)
        {
            Neurons[i].Process();
        }
    }
}
