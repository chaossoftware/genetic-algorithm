using SciML.NeuralNetwork.Activation;
using System.Collections.Generic;

namespace SciML.NeuralNetwork.Entities;

/// <summary>
/// Describes base neuron of hidden layer with activation function.
/// </summary>
public sealed class HiddenNeuron : INeuron<HiddenNeuron>
{
    private readonly IActivationFunction _activationFunction;

    /// <summary>
    /// Initializes a new instance of the <see cref="HiddenNeuron"/> class 
    /// with specified activation function.
    /// </summary>
    /// <param name="activationFunction">activation function instance</param>
    public HiddenNeuron(IActivationFunction activationFunction)
    {
        Inputs = new List<Synapse>();
        Outputs = new List<Synapse>();
        _activationFunction = activationFunction;
    }

    /// <summary>
    /// Gets or sets list of input synapses.
    /// </summary>
    public List<Synapse> Inputs { get; set; }

    /// <summary>
    /// Gets or sets list of output synapses.
    /// </summary>
    public List<Synapse> Outputs { get; set; }

    /// <summary>
    /// Makes a copy of the neuron with the same activation function.
    /// </summary>
    /// <returns>neuron copy</returns>
    public object Clone() =>
        new HiddenNeuron(_activationFunction);

    /// <summary>
    /// Processes input signals through the neuron (processes all inputs via 
    /// activation function and multiplies to weight).
    /// </summary>
    public void Process()
    {
        double arg = 0;

        for (int i = 0; i < Inputs.Count; i++)
        {
            arg += Inputs[i].Signal;
        }

        arg = _activationFunction.Phi(arg);

        foreach (Synapse synapse in Outputs)
        {
            synapse.Signal = synapse.Weight * arg;
        }
    }
}
