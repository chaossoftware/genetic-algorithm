using System.Collections.Generic;

namespace SciML.NeuralNetwork.Entities;

/// <summary>
/// Describes base neuron of input layer.
/// </summary>
public sealed class InputNeuron : INeuron<InputNeuron>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InputNeuron"/> class.
    /// </summary>
    public InputNeuron()
    {
        Outputs = new List<Synapse>();
    }

    /// <summary>
    /// Gets or sets input synapse.
    /// </summary>
    public Synapse Input { get; set; }

    /// <summary>
    /// Gets or sets list of output synapses.
    /// </summary>
    public List<Synapse> Outputs { get; set; }

    /// <summary>
    /// Makes a copy of the neuron.
    /// </summary>
    /// <returns>neuron copy</returns>
    public object Clone() =>
        new InputNeuron();

    /// <summary>
    /// Processes input signal through the neuron to outputs 
    /// (multiplies input by weight).
    /// </summary>
    public void Process()
    {
        foreach (Synapse output in Outputs)
        {
            output.Signal = Input.Signal * output.Weight;
        }
    }
}
