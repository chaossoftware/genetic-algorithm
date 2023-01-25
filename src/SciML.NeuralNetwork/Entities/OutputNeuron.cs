using System.Collections.Generic;

namespace SciML.NeuralNetwork.Entities;

/// <summary>
/// Describes base neuron of outpul layer.
/// </summary>
public class OutputNeuron : INeuron<OutputNeuron>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OutputNeuron"/> class.
    /// </summary>
    public OutputNeuron()
    {
        Inputs = new List<Synapse>();
    }

    /// <summary>
    /// Gets or sets list of input synapses.
    /// </summary>
    public List<Synapse> Inputs { get; set; }

    /// <summary>
    /// Gets or sets output synapse.
    /// </summary>
    public Synapse Output { get; set; }

    /// <summary>
    /// Makes a copy of the neuron.
    /// </summary>
    /// <returns>neuron copy</returns>
    public virtual object Clone() =>
        new OutputNeuron();

    /// <summary>
    /// Processes input signals through the neuron (just sum of inputs).
    /// </summary>
    public virtual void Process()
    {
        Output.Signal = 0;

        for (int i = 0; i < Inputs.Count; i++)
        {
            Output.Signal += Inputs[i].Signal;
        }
    }
}
